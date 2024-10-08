using NebulaGraphDemo.Dto;
using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;

namespace NebulaGraphDemo.Repository.Data;

public class PostUserCommentRelationRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);

    public async Task CreateBelongsToEdgeAsync()
    {
        var query = "CREATE EDGE IF NOT EXISTS belongs_to();";

        await _queryExecutor.ExecuteAsync(query);
    }
    
    public async Task CreateCommentEdgeAsync()
    {
        var query = "CREATE EDGE IF NOT EXISTS commented();";

        await _queryExecutor.ExecuteAsync(query);
    }

    public async Task AddCommentBelongsToEdgeAsync(string commentUUid, string postUUid)
    {
        var query = $"INSERT EDGE belongs_to () VALUES '{commentUUid}'->'{postUUid}':();";

        await _queryExecutor.ExecuteAsync(query);
    }

    public async Task AddUserCommentedEdgeAsync(string username, string commentUUid)
    {
        var query = $"INSERT EDGE commented () VALUES '{username}'->'{commentUUid}':();";

        await _queryExecutor.ExecuteAsync(query);
    }

    public async Task AddCommentLikeEdgeAsync(string username, string commentUUid)
    {
        var regDateTimeFormatted = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

        var query = $"INSERT EDGE like (likeDateTime) VALUES '{username}'->'{commentUUid}':(datetime('{regDateTimeFormatted}'));";

        await _queryExecutor.ExecuteAsync(query);
    }

    public async Task<List<Dto.Comment>> GetPostCommentsAsync(string postUUid)
    {
        //var query = $"MATCH (c:comment)-[:belongs_to]->(p:post {{Uuid: '{postUUid}'}}) RETURN c;";
        var query = @$"MATCH (u:user)-[:commented]->(c:comment)-[:belongs_to]->(p:post {{uuid: '{postUUid}'}})
                    RETURN
                       u.user.Username as RegUser,
                       c.comment.Content as Content,
                       c.comment.ContentTypeId as ContentTypeId,
                       c.comment.RegDateTime as RegDateTime,
                       c.comment.ChildCount as ChildCount,
                       c.comment.Depth as Depth,
                       c.comment.Uuid as Uuid,
                       c.comment.ParentUuid as ParentUuid,
                       c.comment.ContentUuid as ContentUuid,
                       c.comment.CommentContentUuid as CommentContentUuid
                    ";
                            
        var result = await _queryExecutor.ExecuteAsync(query);

        var comments = GenericNebulaDataConverter2.ConvertToEntityList<Dto.Comment>(result);
        return comments;
    }

    public async Task<List<Dto.Comment>> GetUserCommentsAsync(string username)
    {
        var query = $"MATCH (u:user {{Username: '{username}'}})-[:commented]->(c:comment) RETURN c;";
        var result = await _queryExecutor.ExecuteAsync(query);

        var comments = GenericNebulaDataConverter.ConvertToEntityList<Dto.Comment>(result);

        foreach (var cm in comments)
        {
            cm.RegUser = username;
        }
        
        return comments;
    }

    public async Task<List<CommentLikeDto>> GetCommentLikesAsync(string commentUUid)
    {
        var query = @$"MATCH (u:user)-[:like]->(c:comment {{Uuid : '{commentUUid}'}}) 
                       RETURN
                       u.user.Username as Username,
                       u.user.Fullname as Fullname,
                       u.user.ProfilePictureId as ProfilePictureId";
        
        var result = await _queryExecutor.ExecuteAsync(query);

        var likes = GenericNebulaDataConverter2.ConvertToEntityList<CommentLikeDto>(result);
        
        return likes;
    }

    public async Task<List<CommentsLikedByDto>> GetCommentsLikedByAsync(string username)
    {
        var query = @$"MATCH (u1:user {{Username: '{username}'}})-[:like]->(c:comment)-[:belongs_to]->(p:post)
                        MATCH (u2:user)-[:commented]->(c)
                        MATCH (u3:user)-[:register]->(p)
                        RETURN 
                            c.comment.Content as Content,
                            c.comment.ContentTypeId as ContentTypeId,
                            c.comment.RegDateTime as RegDateTime,
                            c.comment.ChildCount as ChildCount,
                            c.comment.Depth as Depth,
                            c.comment.Uuid as Uuid,
                            c.comment.ParentUuid as ParentUuid,
                            c.comment.ContentUuid as ContentUuid,
                            c.comment.CommentContentUuid as CommentContentUuid,
                            p.post.Content as PostContent,
                            p.post.Attachments as PostAttachments,
                            u2.user.Username as CommentRegUser,
                            u2.user.Fullname as CommentRegUserFullname,
                            u2.user.ProfilePictureId as CommentRegUserProfilePictureId,
                            u3.user.Username as PostRegUser,
                            u3.user.Fullname as PostRegUserFullname,
                            u3.user.ProfilePictureId as PostRegUserProfilePictureId;";
        
        var result = await _queryExecutor.ExecuteAsync(query);

        var comments = GenericNebulaDataConverter2.ConvertToEntityList<CommentsLikedByDto>(result);
        
        return comments;
    }
}