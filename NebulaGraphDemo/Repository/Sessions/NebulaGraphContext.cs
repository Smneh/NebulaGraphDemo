using NebulaNet;

namespace NebulaGraphDemo.Repository.Sessions;

public class NebulaSessionManager : IDisposable
{
    private readonly NebulaConnection _connection;
    private long _sessionId;

    public NebulaSessionManager(string ip, int port)
    {
        _connection = new NebulaConnection();
        OpenConnectionAsync(ip, port).Wait();
        _sessionId = GetSessionIdAsync().Result;
    }

    private async Task OpenConnectionAsync(string ip, int port)
    {
        try
        {
            await _connection.OpenAsync(ip, port);
            Console.WriteLine($"Connection opened to Nebula Graph at {ip}:{port}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening connection: {ex.Message}");
            throw;
        }
    }

    public async Task<long> GetSessionIdAsync()
    {
        if (_sessionId == 0)
        {
            var authResponse = await _connection.AuthenticateAsync("root", "root");
            _sessionId = authResponse.Session_id;
            Console.WriteLine($"Authenticated, session ID: {_sessionId}");
        }
        return _sessionId;
    }
        
    public async Task UseSpaceAsync(string spaceName)
    {
        var sessionId = await GetSessionIdAsync();
        var useSpaceQuery = $"USE {spaceName}";
        await _connection.ExecuteAsync(sessionId, useSpaceQuery);
    }
        


    public async Task EnsureSpaceExistsAsync(string spaceName)
    {
        var statement = $"CREATE SPACE IF NOT EXISTS {spaceName} (vid_type=FIXED_STRING(40), replica_factor=1, partition_num=10);";
        await _connection.ExecuteAsync(_sessionId, statement);
        Console.WriteLine($"Space '{spaceName}' created successfully.");
    }
    public NebulaConnection GetConnection()
    {
        return _connection;
    }

    public void Dispose()
    {
        _connection.Dispose(); // Ensure the connection is closed properly
    }
}