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
            RegDate = input.RegDate,
            RegTime = input.RegTime,
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
            Uuid = input.Uuid,
            ContentUuid = input.ContentUuid,
            ParentUuid = input.ParentUuid,
            Edited = input.Edited,
            EditDateTime = input.EditDateTime,
            Attachments = input.Attachments,
            RegUser = input.RegUser,
            UniqueId = input.UniqueId,
            RegDateTime = input.RegDateTime,
            LikeCount = input.LikeCount,
            CommentCount = input.CommentCount,
            ViewCount = input.ViewCount,

        };
        await postRepository.CreatePostAsync(post.ToDto());
        return post;
    }
}
