using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;
using NebulaNet;
using Thrift.Protocol;

namespace NebulaGraphDemo.Repository.Data;

public class CommentRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);

    public async Task CreateCommentTagAsync()
    {
        var query = @"CREATE TAG IF NOT EXISTS comment(
                    Content string,
                    ContentTypeId int32,
                    RegDateTime datetime,
                    ChildCount int32,
                    Depth int64,
                    Uuid string,
                    ParentUuid string,
                    ContentUuid string,
                    CommentContentUuid string
                );";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"comment Tag created, Result: {result}");
    }

    public async Task CreateIndexForCommentAsync()
    {
        //var statement = "CREATE TAG INDEX `comment` on `comment`(`uuid`(40))";
        var statement = "CREATE TAG INDEX IF NOT EXISTS comment_index ON comment();";
        await _queryExecutor.ExecuteAsync(statement);
    }

    public async Task CreateCommentAsync(Comment comment)
    {
        var regDateTimeFormatted = comment.RegDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

        var query = $@"
        INSERT VERTEX comment (
            Content, 
            ContentTypeId, 
            RegDateTime, 
            ChildCount, 
            Depth, 
            Uuid, 
            ParentUuid, 
            ContentUuid, 
            CommentContentUuid
          ) 
        VALUES '{comment.Uuid}': (
            '{comment.Content}', 
            {comment.ContentTypeId}, 
            datetime(""{regDateTimeFormatted}""), 
            {comment.ChildCount}, 
            {comment.Depth}, 
            '{comment.Uuid}', 
            '{comment.ParentUuid}', 
            '{comment.ContentUuid}', 
            '{comment.CommentContentUuid}'
          );";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"comment created: {comment.Uuid}, Result: {result}");
    }
    
    public async Task<NebulaGraphDemo.Dto.Comment?> GetCommentAsync(string uuid)
    {
        var query = $"MATCH (u:comment{{Uuid:'{uuid}'}}) RETURN u";
        var result = await _queryExecutor.ExecuteAsync(query);

        var comments = GenericNebulaDataConverter.ConvertToEntityList<NebulaGraphDemo.Dto.Comment>(result);

        return comments.FirstOrDefault();
    }

    public async Task<List<NebulaGraphDemo.Dto.Comment>> GetAllCommentsAsync()
    {
        var query = "MATCH (p:comment) RETURN p";
        var result = await _queryExecutor.ExecuteAsync(query);
        var comments = new List<NebulaGraphDemo.Dto.Comment>();

        comments = GenericNebulaDataConverter.ConvertToEntityList<NebulaGraphDemo.Dto.Comment>(result);

        return comments;
    }
}