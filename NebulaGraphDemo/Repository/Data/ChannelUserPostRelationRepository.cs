using NebulaGraphDemo.Dto;
using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;

namespace NebulaGraphDemo.Repository.Data;

public class ChannelUserPostRelationRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);
    
    public async Task CreateFollowEdgeAsync()
    {
        var query = "CREATE EDGE IF NOT EXISTS follow(followDateTime datetime);";

        await _queryExecutor.ExecuteAsync(query);
    }    
    
    public async Task CreateAdminOfEdgeAsync()
    {
        var query = "CREATE EDGE IF NOT EXISTS admin_of();";

        await _queryExecutor.ExecuteAsync(query);
    }  
    
    public async Task AddPostBelongsToEdgeAsync(string postUUid, string issuerId)
    {
        var query = $"INSERT EDGE belongs_to () VALUES '{postUUid}'->'{issuerId}':();";

        await _queryExecutor.ExecuteAsync(query);
    }
    
    public async Task AddUserAdminOfChannelEdgeAsync(string username, string issuerId)
    {
        var query = $"INSERT EDGE admin_of () VALUES '{username}'->'{issuerId}':();";

        await _queryExecutor.ExecuteAsync(query);
    }
    
    public async Task AddPostFollowEdgeAsync(string username, string issuerId)
    {
        var regDateTimeFormatted = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
        
        var query = $"INSERT EDGE follow (followDateTime) VALUES '{username}'->'{issuerId}':(datetime('{regDateTimeFormatted}'));";

        await _queryExecutor.ExecuteAsync(query);
    }
    
    public async Task<List<Post>> GetChannelPostsAsync(string channelId)
    {
        var query = $"MATCH (p:post)-[:belongs_to]->(ch:channel {{channelId: '{channelId}'}}) RETURN p;";
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var posts = GenericNebulaDataConverter.ConvertToEntityList<Post>(result);
        return posts;
    }
    
    public async Task<List<Post>> GetUserPostsAsync(string username)
    {
        var query = $"MATCH (p:post)-[:belongs_to]->(u:user {{Username: '{username}'}}) RETURN p;";
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var posts = GenericNebulaDataConverter.ConvertToEntityList<Post>(result);
        return posts;
    }
    
    public async Task<List<Post>> GetUserTimelinePostsAsync(string username)
    {
        var query = @$"MATCH (u:user {{username: '{username}'}})-[:follow]->(followed)
                        MATCH (p:post)-[:belongs_to]->(followed)
                        RETURN p";
                                
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var posts = GenericNebulaDataConverter.ConvertToEntityList<Post>(result);
        return posts;
    }
    
    public async Task<List<Following>> GetUserFollowingsAsync(string username)
    {
        var query = @$"MATCH (u:user {{username: '{username}'}})-[f:follow]->(followed) 
                        RETURN 
                            CASE 
                                WHEN 'channel' IN tags(followed) THEN 'Channel'
                                WHEN 'user' IN tags(followed) THEN 'User'
                            END AS Type,  

                            CASE 
                                WHEN 'channel' IN tags(followed) THEN followed.channel.channelId
                                WHEN 'user' IN tags(followed) THEN followed.user.username
                            END AS IssuerId,  

                            CASE 
                                WHEN 'channel' IN tags(followed) THEN followed.channel.profilePictureId
                                WHEN 'user' IN tags(followed) THEN followed.user.profilePictureId
                            END AS ProfilePictureId,  

                            CASE 
                                WHEN 'channel' IN tags(followed) THEN followed.channel.title
                                WHEN 'user' IN tags(followed) THEN followed.user.fullname
                            END AS Title,
                            f.followDateTime AS FollowDate;
                    ";
                                
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var feed = GenericNebulaDataConverter2.ConvertToEntityList<Following>(result);
        return feed;
    }    
    
    public async Task<List<Follower>> GetUserFollowersAsync(string username)
    {
        var query = @$"MATCH (follower:user)-[f:follow]->(u:user {{username: '{username}'}}) 
                        RETURN 
                        follower.user.username AS Username,  
                        follower.user.profilePictureId AS ProfilePictureId,  
                        follower.user.fullname AS Fullname,
                        f.followDateTime AS FollowDate;
                    ";
                                
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var feed = GenericNebulaDataConverter2.ConvertToEntityList<Follower>(result);
        return feed;
    }
}