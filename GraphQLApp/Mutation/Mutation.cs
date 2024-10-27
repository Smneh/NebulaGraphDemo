using GraphQL.Dto.Input;
using NebulaGraphDemo.Extensions;
using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Data;

namespace GraphQL.Mutation;

public class Mutation(UserRepository userRepository, PostRepository postRepository, CommentRepository commentRepository, ChannelRepository channelRepository,
    UserPostRelationRepository userPostRelationRepository, ChannelUserPostRelationRepository channelUserPostRelationRepository,
    PostUserCommentRelationRepository postUserCommentRelationRepository)
{
    public async Task<bool> AddUserAdminOfChannelEdgeAsync(string username, string issuerId)
    {
        await channelUserPostRelationRepository.AddUserAdminOfChannelEdgeAsync(username, issuerId);
        return true;
    }

    public async Task<bool> AddFollowEdgeAsync(string username, string issuerId)
    {
        await channelUserPostRelationRepository.AddFollowEdgeAsync(username, issuerId);
        return true;
    }

    public async Task<bool> RemoveUserAdminOfChannelEdgeAsync(string username, string issuerId)
    {
        await channelUserPostRelationRepository.RemoveUserAdminOfChannelEdgeAsync(username, issuerId);
        return true;
    }

    public async Task CreateChannelAsync(Channel channel)
    {
        await channelRepository.CreateChannelAsync(channel);
    }

    public async Task CreateCommentAsync(Comment comment)
    {
        await commentRepository.CreateCommentAsync(comment);
    }
    
    public async Task<bool> AddCommentLikeEdgeAsync(string username, string commentUuid)
    {
        await postUserCommentRelationRepository.AddCommentLikeEdgeAsync(username, commentUuid);
        return true;
    }

    public async Task<bool> AddPostLikeEdgeAsync(string username, string postUuid)
    {
        await userPostRelationRepository.AddPostLikeEdgeAsync(username, postUuid);
        return true;
    }

    public async Task<bool> AddPostVisitEdgeAsync(string username, string postUuid)
    {
        await userPostRelationRepository.AddPostVisitEdgeAsync(username, postUuid);
        return true;
    }

    public async Task UpdateUserAsync(User user)
    {
        await userRepository.UpdateUserAsync(user);
    }

    public async Task<User> CreateUser(CreateUserInput input)
    {
        var user = new User
        {
            Username = input.Username,
            WorkspaceTitle = input.WorkspaceTitle,
            WorkspaceId = input.WorkspaceId,
            Fullname = input.Fullname,
            FatherName = input.FatherName,
            LastModifyDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            IsMale = input.IsMale,
            NationalId = input.NationalId,
            Address = input.Address,
            Tel = input.Tel,
            Email = input.Email,
            Mobile = input.Mobile,
            ProfilePictureId = input.ProfilePictureId,
            WallpaperPictureId = input.WallpaperPictureId,
            Bio = input.Bio,
            Description = input.Description,
            OrgCode = input.OrgCode,
            OrganizationalUnitId = input.OrganizationalUnitId,
            OrgInternalPhone = input.OrgInternalPhone,
            Id = 1,
            ProjectColumns = input.ProjectColumns
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