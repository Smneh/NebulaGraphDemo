using NebulaGraphDemo.Dto;
using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;
using Comment = NebulaGraphDemo.Models.Comment;

namespace NebulaGraphDemo.Repository.Data;

public class UserPostRelationRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);
    
    public async Task CreateRegisterEdgeAsync()
    {
        var query = "CREATE EDGE IF NOT EXISTS register();";

        await _queryExecutor.ExecuteAsync(query);
    }       
    
    public async Task CreateLikeEdgeAsync()
    {
        var query = "CREATE EDGE IF NOT EXISTS like(likeDateTime datetime);";

        await _queryExecutor.ExecuteAsync(query);
    }   
    
    public async Task CreateVisitEdgeAsync()
    {
        var query = "CREATE EDGE IF NOT EXISTS visit(visitDateTime datetime);";

        await _queryExecutor.ExecuteAsync(query);
    }    
    
    public async Task CreateCommentEdgeAsync()
    {
        var query = @"CREATE EDGE IF NOT EXISTS 
                      comment(RegDateTime datetime, Content string, ContentTypeId int32, ChildCount int32, Depth int64,
                      Uuid string, ParentUuid string, ContentUuid string, CommentContentUuid string);";

        await _queryExecutor.ExecuteAsync(query);
    }   
    
    public async Task AddPostRegisterEdgeAsync(string username, string postUUid)
    {
        var query = $"INSERT EDGE register () VALUES '{username}'->'{postUUid}':();";

        await _queryExecutor.ExecuteAsync(query);
    } 
    
    public async Task AddPostLikeEdgeAsync(string username, string postUUid)
    {
        var regDateTimeFormatted = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

        var query = $"INSERT EDGE like (likeDateTime) VALUES '{username}'->'{postUUid}':(datetime('{regDateTimeFormatted}'));";

        await _queryExecutor.ExecuteAsync(query);
    }
    
    public async Task AddPostVisitEdgeAsync(string username, string postUUid)
    {
        var regDateTimeFormatted = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

        var query = $"INSERT EDGE visit (visitDateTime) VALUES '{username}'->'{postUUid}':(datetime('{regDateTimeFormatted}'));";

        await _queryExecutor.ExecuteAsync(query);
    }
    
    
    public async Task AddPostCommentEdgeAsync(string username, string postUUid, Comment comment)
    {
        var regDateTimeFormatted = comment.RegDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

        var query = @$"INSERT EDGE comment (RegDateTime, Content, ContentTypeId, ChildCount, Depth, Uuid, ParentUuid, ContentUuid, CommentContentUuid) 
                       VALUES '{username}'->'{postUUid}':(datetime('{regDateTimeFormatted}'), '{comment.Content}', {comment.ContentTypeId},
                       {comment.ChildCount}, {comment.Depth}, '{comment.Uuid}', '{comment.ParentUuid}', '{comment.ContentUuid}', '{comment.CommentContentUuid}');";

        await _queryExecutor.ExecuteAsync(query);
    }
    
    public async Task<List<User>> GetUsersWhoLikePostAsync(string postUUid)
    {
        var query = $"MATCH (u:user)-[:like]->(p:post {{Uuid: '{postUUid}'}}) RETURN u;";
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var users = GenericNebulaDataConverter.ConvertToEntityList<User>(result);
        return users;
    }

    public async Task<List<Dto.Post>> GetPostsRegisteredByUserAsync(string username)
    {
        var query = $"MATCH (u:user {{Username: '{username}'}})-[:register]->(p:post) RETURN p;";
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var posts = GenericNebulaDataConverter.ConvertToEntityList<Dto.Post>(result);
        return posts;
    }

    public async Task<List<User>> GetUsersWhoVisitPostAsync(string postUUid)
    {
        var query = $"MATCH (u:user)-[:like]->(p:post {{Uuid: '{postUUid}'}}) RETURN u;";
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var users = GenericNebulaDataConverter.ConvertToEntityList<User>(result);
        return users;
    }

    public async Task<List<NebulaGraphDemo.Dto.Comment>> GetPostCommentsAsync(string postUUid)
    {
        var query = $"GO FROM '{postUUid}' OVER comment REVERSELY YIELD edge AS e";
        var result = await _queryExecutor.ExecuteAsync(query);

        var comments = GenericNebulaDataConverter.ConvertToEntityList<NebulaGraphDemo.Dto.Comment>(result);
        return comments;
    }
    
    public async Task<List<Dto.Post>> GetPostsLikedByUserAsync(string username)
    {
        var query = $"MATCH (u:user{{Username: '{username}'}})-[:like]->(p:post) RETURN p;";
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var users = GenericNebulaDataConverter.ConvertToEntityList<Dto.Post>(result);
        return users;
    }

    
    public async Task<List<PostsVisitedByUserDto>> GetPostsVisitedByUserAsync(string username)
    {
        var query = @$"MATCH (u:user{{Username: '{username}'}})-[:visit]->(p:post)
                        MATCH (u2:user)-[:register]->(p)
                        RETURN 
                        p.post.UUid as Uuid,
                        p.post.IssuerId as IssuerId,
                        p.post.Attachments as Attachments,
                        p.post.Content as Content,
                        p.post.RegDateTime as RegDateTime,
                        u2.user.Username as PostRegUser,
                        u2.user.Fullname as PostRegUserFullname,
                        u2.user.ProfilePictureId as ProfilePictureId;";
        
        var result = await _queryExecutor.ExecuteAsync(query);
        
        var posts = GenericNebulaDataConverter2.ConvertToEntityList<PostsVisitedByUserDto>(result);
        return posts;
    }

}