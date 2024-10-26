using GraphQL.Dto.Input;
using NebulaGraphDemo.Extensions;
using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Data;

namespace GraphQL.Mutation;

public class Mutation(UserRepository userRepository, PostRepository postRepository)
{
    public async Task<User> CreateUser(CreateUserInput input)
    {
        var user = new User
        {
            Username = input.Username,
            Fullname = input.Fullname,
            Email = input.Email,
        };
        await userRepository.CreateUserAsync(user);
        return user;
    }

    public async Task<PostEntity> CreatePost(CreatePostInput input)
    {
        var post = new PostEntity
        {
            IssuerId = input.IssuerId,
            IssuerType = input.IssuerType,
            RegDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            RegTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            IsShareable = input.IsShareable,
            IsCommentable = input.IsCommentable,
            IsPublic = input.IsPublic,
            PostType = input.PostType,
            Content = input.Content,
            IssuerPostId = input.IssuerPostId,
            IsSurvey = input.IsSurvey,
            PostTypeId = input.PostTypeId,
            ParentIssuerType = input.ParentIssuerType,
            SurveyId = input.SurveyId,
            ShareCount = input.ShareCount,
            Uuid = Guid.NewGuid().ToString(),
            ContentUuid = Guid.NewGuid().ToString(),
            ParentUuid = input.ParentUuid,
            Edited = input.Edited,
            EditDateTime = null,
            Attachments = input.Attachments,
            RegUser = input.RegUser,
            UniqueId = Guid.NewGuid().ToString(),
            RegDateTime = DateTime.Now,
            LikeCount = input.LikeCount,
            CommentCount = input.CommentCount,
            ViewCount = input.ViewCount,

        };
        await postRepository.CreatePostAsync(post.ToDto());
        return post;
    }
}
