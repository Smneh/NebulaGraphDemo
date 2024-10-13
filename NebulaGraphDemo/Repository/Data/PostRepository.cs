using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;


namespace NebulaGraphDemo.Repository.Data;

public class PostRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);

    public async Task CreatePostTagAsync()
    {
        var query = @"CREATE TAG IF NOT EXISTS post(
                    issuerId string,
                    issuerType int32,
                    regDate int64,
                    regTime int64,
                    isShareable bool,
                    isCommentable bool,
                    isPublic bool,
                    postType int32,
                    regUser string,
                    uniqueId string,
                    regDateTime datetime,
                    likeCount int32,
                    commentCount int32,
                    viewCount int32,
                    content string,
                    issuerPostId int32,
                    isSurvey bool,
                    postTypeId int64,
                    parentIssuerType string,
                    surveyId int64,
                    shareCount int32,
                    uuid string,
                    contentUuid string,
                    parentUuid string,
                    edited bool,
                    editDateTime datetime,
                    attachments string
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
            issuerId, issuerType, regDate, regTime, isShareable, 
            isCommentable, isPublic, postType, regUser, uniqueId, 
            regDateTime, likeCount, commentCount, viewCount, content, 
            issuerPostId, isSurvey, postTypeId, parentIssuerType, 
            surveyId, shareCount, uuid, contentUuid, parentUuid, 
            edited, editDateTime, attachments) 
        VALUES '{post.uuid}': (
            '{post.IssuerId}', {post.IssuerType}, {post.RegDate}, {post.RegTime}, {post.IsShareable}, 
            {post.IsCommentable}, {post.IsPublic}, {post.PostType}, '{post.RegUser}', '{post.UniqueId}',
            datetime(""{regDateTimeFormatted}""), {post.LikeCount}, {post.CommentCount}, {post.ViewCount}, '{post.Content}', 
            {post.IssuerPostId}, {post.IsSurvey}, {post.PostTypeId}, '{post.ParentIssuerType}',
            {post.SurveyId}, {post.ShareCount}, '{post.uuid}', '{post.ContentUuid}', '{post.ParentUuid}', 
            {post.Edited}, datetime(""{editDateTimeFormatted}""), '{post.Attachments}');";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"post created: {post.uuid}, Result: {result}");
    }
    
    public async Task<Dto.Post?> GetPostAsync(string uuid)
    {
        var query = $"MATCH (u:post{{uuid:'{uuid}'}}) RETURN u";
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