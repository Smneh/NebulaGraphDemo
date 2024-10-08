using Nebula.Common;
using NebulaGraphDemo.Repository.Sessions;
using NebulaNet;

namespace NebulaGraphDemo.Repository.Data;

public class NebulaQueryExecutor(NebulaSessionManager sessionManager)
{
    // public async Task<object?> ExecuteAsync(string statement)
    // {
    //     try
    //     {
    //         var sessionId = await sessionManager.GetSessionIdAsync();
    //         var connection = sessionManager.GetConnection();
    //         
    //         var result = await connection.ExecuteAsync(sessionId, statement);
    //         return result;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Query execution error: {ex.Message}");
    //         return null;
    //     }
    // }        
        
    public async Task<DataSet> ExecuteAsync(string statement)
    {
        try
        {
            var sessionId = await sessionManager.GetSessionIdAsync();
            var connection = sessionManager.GetConnection();
                
            var executionResponse = await connection.ExecuteAsync(sessionId, statement);
            if (executionResponse.Error_code != 0)
            {
                throw new Exception($"Nebula query failed: {executionResponse.Error_msg}");
            }

            return executionResponse.Data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Query execution error: {ex.Message}");
            return null;
        }
    }
}