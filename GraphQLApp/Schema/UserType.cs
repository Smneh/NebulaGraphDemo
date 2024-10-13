using HotChocolate.Types;
using NebulaGraphDemo.Models;

namespace GraphQL.Schema;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Field(u => u.Username).Type<NonNullType<StringType>>();
        descriptor.Field(u => u.Fullname).Type<StringType>();
        descriptor.Field(u => u.Email).Type<StringType>();
    }
}