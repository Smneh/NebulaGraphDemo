using System.Collections;
using System.Reflection;
using System.Text;
using Nebula.Common;
using DateTime = System.DateTime;

namespace NebulaGraphDemo.Utilities;

public static class GenericNebulaDataConverter2
{
    public static List<T> ConvertToEntityList<T>(DataSet dataSet) where T : new()
    {
        var entities = new List<T>();
        var columns = new Dictionary<int, string>();

        for (int i = 0; i < dataSet.Column_names.Count; i++)
        {
            var columnName = (DecodeAsciiValues(dataSet.Column_names[i]));
            columns.Add(i, columnName);
        }

        foreach (var row in dataSet.Rows)
        {
            var entity = ConvertToEntity<T>(row, columns);
            if (entity != null)
            {
                entities.Add(entity);
            }
        }

        return entities;
    }

    public static T ConvertToEntity<T>(Row row, Dictionary<int, string> columns) where T : new()
    {
        var entity = new T();
        var properties = typeof(T).GetProperties();

        var fullname = typeof(T).ToString();
        var name = fullname.Split('.').Last().ToLower();

        for (var i = 0; i < row.Values.Count; i++)
        {
            var value = row.Values[i];
            if (value.VVal is Vertex vertex)
            {
                foreach (var tag in vertex.Tags)
                {
                    if (ByteArrayEquals(tag.Name, Encoding.ASCII.GetBytes(name)))
                    {
                        foreach (var prop in properties)
                        {
                            var propName = LowerFirstLetter(prop.Name);
                            var propNameBytes = Encoding.ASCII.GetBytes(propName);
                            var matchingProp = tag.Props.FirstOrDefault(p => ByteArrayEquals(p.Key, propNameBytes));
                            if (matchingProp.Key != null)
                            {
                                SetPropertyValue(entity, prop, matchingProp.Value);
                            }
                        }
                    }
                }
            }

            if (value.EVal is Edge edge)
            {
                if (ByteArrayEquals(edge.Name, Encoding.ASCII.GetBytes(name)))
                {
                    foreach (var prop in properties)
                    {
                        var propName = LowerFirstLetter(prop.Name);
                        var propNameBytes = Encoding.ASCII.GetBytes(propName);
                        var matchingProp = edge.Props.FirstOrDefault(p => ByteArrayEquals(p.Key, propNameBytes));
                        if (matchingProp.Key != null)
                        {
                            SetPropertyValue(entity, prop, matchingProp.Value);
                        }
                    }
                }
            }
            else
            {
                columns.TryGetValue(i, out var columnName);
                var prop = properties.FirstOrDefault(p => p.Name.ToLower() == columnName?.ToLower());
                if (prop != null)
                {
                    SetPropertyValue(entity, prop, value);
                }
            }
        }

        return entity;
    }

    private static void SetPropertyValue<T>(T entity, PropertyInfo prop, Value value)
    {
        object convertedValue;

        if (value == null)
        {
            convertedValue = null;
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            convertedValue = Convert.ToInt32(GetValueAsLong(value));
        }
        else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
        {
            convertedValue = GetValueAsLong(value);
        }
        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
        {
            convertedValue = GetValueAsDouble(value);
        }
        else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
        {
            convertedValue = Convert.ToDecimal(GetValueAsDouble(value));
        }
        else if (prop.PropertyType == typeof(string))
        {
            convertedValue = GetValueAsString(value);
        }
        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
        {
            convertedValue = GetValueAsBool(value);
        }
        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
        {
            convertedValue = GetValueAsDateTime(value);
        }
        else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var listType = prop.PropertyType;
            var elementType = listType.GenericTypeArguments[0];

            var listInstance = (IList)Activator.CreateInstance(listType);

            foreach (var item in value.LVal.Values)
            {
                var map = item.MVal;
                var kvs = map.Kvs;
                var elementInstance = Activator.CreateInstance(elementType);

                foreach (var elementProp in elementType.GetProperties())
                {
                    var propName = LowerFirstLetter(elementProp.Name);
                    var propNameBytes = Encoding.ASCII.GetBytes(propName);
                    var matchingProp = kvs.FirstOrDefault(p => ByteArrayEquals(p.Key, propNameBytes));
                    if (matchingProp.Key != null)
                    {
                        SetPropertyValue(elementInstance, elementProp, matchingProp.Value);
                    }
                }

                listInstance.Add(elementInstance);
            }

            convertedValue = listInstance;
        }
        else
        {
            throw new NotSupportedException($"Unsupported property type: {prop.PropertyType}");
        }

        prop.SetValue(entity, convertedValue);
    }

    private static void SetPropertyValue<T>(T entity, PropertyInfo prop, object value)
    {
        object convertedValue;

        if (value == null)
        {
            convertedValue = null;
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            convertedValue = Convert.ToInt32(value);
        }
        else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
        {
            convertedValue = Convert.ToInt64(value);
        }
        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
        {
            convertedValue = Convert.ToDouble(value);
        }
        else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
        {
            convertedValue = Convert.ToDecimal(value);
        }
        else if (prop.PropertyType == typeof(string))
        {
            convertedValue = value.ToString();
        }
        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
        {
            convertedValue = Convert.ToBoolean(value);
        }
        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
        {
            convertedValue = Convert.ToDateTime(value);
        }
        else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
        {
            // Handle generic List<>
            var listType = prop.PropertyType;
            var elementType = listType.GenericTypeArguments[0];
            var listInstance = (IList)Activator.CreateInstance(listType);

            foreach (var item in value as IEnumerable)
            {
                var elementInstance = Activator.CreateInstance(elementType);

                foreach (var elementProp in elementType.GetProperties())
                {
                    var itemProp = item.GetType().GetProperty(elementProp.Name);
                    if (itemProp != null)
                    {
                        var itemValue = itemProp.GetValue(item);

                        // Recursive call to set properties using the object overload
                        SetPropertyValue(elementInstance, elementProp, itemValue);
                    }
                }

                listInstance.Add(elementInstance);
            }

            convertedValue = listInstance;
        }
        else
        {
            throw new NotSupportedException($"Unsupported property type: {prop.PropertyType}");
        }

        prop.SetValue(entity, convertedValue);
    }

    private static long GetValueAsLong(Value value)
    {
        return Convert.ToInt64(value.GetType().GetProperty("IVal")?.GetValue(value));
    }

    private static double GetValueAsDouble(Value value)
    {
        return Convert.ToDouble(value.GetType().GetProperty("FVal")?.GetValue(value));
    }

    private static string GetValueAsString(Value value)
    {
        var sValProperty = value.GetType().GetProperty("SVal");
        if (sValProperty != null)
        {
            var byteArray = sValProperty.GetValue(value) as byte[];
            if (byteArray != null && byteArray.Length > 0)
            {
                return Encoding.ASCII.GetString(byteArray);
            }
        }

        return null;
    }

    private static bool GetValueAsBool(Value value)
    {
        return Convert.ToBoolean(value.GetType().GetProperty("BVal")?.GetValue(value));
    }

    private static DateTime GetValueAsDateTime(Value value)
    {
        var nebulaDateTime = value.GetType().GetProperty("DtVal")?.GetValue(value) as Nebula.Common.DateTime;

        if (nebulaDateTime != null)
        {
            var year = nebulaDateTime.Year;
            var month = nebulaDateTime.Month;
            var day = nebulaDateTime.Day;
            var hour = nebulaDateTime.Hour;
            var minute = nebulaDateTime.Minute;
            var second = nebulaDateTime.Sec;
            var microsecond = nebulaDateTime.Microsec;

            var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc)
                .AddTicks(microsecond * 10);

            return dateTime;
        }

        throw new InvalidCastException($"Unable to cast value : {nebulaDateTime} to Nebula.Common.DateTime.");
    }

    private static bool ByteArrayEquals(byte[] a1, byte[] a2)
    {
        if (a1 == null || a2 == null || a1.Length != a2.Length)
            return false;
        for (var i = 0; i < a1.Length; i++)
            if (a1[i] != a2[i])
                return false;
        return true;
    }

    static string DecodeAsciiValues(byte[] asciiValues)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (int asciiValue in asciiValues)
        {
            stringBuilder.Append((char)asciiValue);
        }

        return stringBuilder.ToString();
    }

    static string LowerFirstLetter(string propName)
    {
        if (string.IsNullOrEmpty(propName) || char.IsLower(propName[0]))
            return propName; // Return as is if the first letter is already lowercase

        return char.ToLower(propName[0]) + propName.Substring(1);
    }
}