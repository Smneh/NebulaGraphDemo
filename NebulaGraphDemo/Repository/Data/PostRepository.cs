using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;


namespace NebulaGraphDemo.Repository.Data;

public class PostRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);

    public async Task CreatePostTagAsync()
    {
        var query = @"CREATE TAG IF NOT EXISTS post(
                    IssuerId string,
                    IssuerType int32,
                    RegDate int64,
                    RegTime int64,
                    IsShareable bool,
                    IsCommentable bool,
                    IsPublic bool,
                    PostType int32,
                    RegUser string,
                    UniqueId string,
                    RegDateTime datetime,
                    LikeCount int32,
                    CommentCount int32,
                    ViewCount int32,
                    Content string,
                    IssuerPostId int32,
                    IsSurvey bool,
                    PostTypeId int64,
                    ParentIssuerType string,
                    SurveyId int64,
                    ShareCount int32,
                    Uuid string,
                    ContentUuid string,
                    ParentUuid string,
                    Edited bool,
                    EditDateTime datetime,
                    Attachments string
                );";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"post Tag created, Result: {result}");
    }

    public async Task CreateIndexForPostAsync()
    {
        //var statement = "CREATE TAG INDEX `post` on `post`(`uuid`(40))";
        var statement = "CREATE TAG INDEX IF NOT EXISTS post_index ON post();";
        await _queryExecutor.ExecuteAsync(statement);
    }

    public async Task CreatePostAsync(Dto.Post post)
    {
        var regDateTimeFormatted = post.RegDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
        var editDateTimeFormatted = post.EditDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

        var query = $@"
        INSERT VERTEX post (
            IssuerId, IssuerType, RegDate, RegTime, IsShareable, 
            IsCommentable, IsPublic, PostType, RegUser, UniqueId, 
            RegDateTime, LikeCount, CommentCount, ViewCount, Content, 
            IssuerPostId, IsSurvey, PostTypeId, ParentIssuerType, 
            SurveyId, ShareCount, Uuid, ContentUuid, ParentUuid, 
            Edited, EditDateTime, Attachments) 
        VALUES '{post.Uuid}': (
            '{post.IssuerId}', {post.IssuerType}, {post.RegDate}, {post.RegTime}, {post.IsShareable}, 
            {post.IsCommentable}, {post.IsPublic}, {post.PostType}, '{post.RegUser}', '{post.UniqueId}',
            datetime(""{regDateTimeFormatted}""), {post.LikeCount}, {post.CommentCount}, {post.ViewCount}, '{post.Content}', 
            {post.IssuerPostId}, {post.IsSurvey}, {post.PostTypeId}, '{post.ParentIssuerType}',
            {post.SurveyId}, {post.ShareCount}, '{post.Uuid}', '{post.ContentUuid}', '{post.ParentUuid}', 
            {post.Edited}, datetime(""{editDateTimeFormatted}""), '{post.Attachments}');";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"post created: {post.Uuid}, Result: {result}");
    }
    
    public async Task<Dto.Post?> GetPostAsync(string uuid)
    {
        var query = $"MATCH (u:post{{Uuid:'{uuid}'}}) RETURN u";
        var result = await _queryExecutor.ExecuteAsync(query);

        var posts = GenericNebulaDataConverter.ConvertToEntityList<Dto.Post>(result);

        return posts.FirstOrDefault();
    }

    public async Task<List<Dto.Post>> GetAllPostsAsync()
    {
        var query = "MATCH (p:post) RETURN p";
        var result = await _queryExecutor.ExecuteAsync(query);
        var posts = new List<Dto.Post>();

        posts = GenericNebulaDataConverter.ConvertToEntityList<Dto.Post>(result);

        return posts;
    }
}