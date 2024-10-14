using NebulaGraphDemo.Dto;
using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Data;
using Post = NebulaGraphDemo.Dto.Post;

namespace GraphQL.Query;

public class Query(UserRepository userRepository, PostRepository postRepository, CommentRepository commentRepository, ChannelRepository channelRepository,
    UserPostRelationRepository userPostRelationRepository, ChannelUserPostRelationRepository channelUserPostRelationRepository,
    PostUserCommentRelationRepository postUserCommentRelationRepository)
{
    // User-related queries
    public async Task<List<User>> GetUsers() => await userRepository.GetAllUsersAsync();

    public async Task<User?> GetUser(string username) => await userRepository.GetUserAsync(username);

    public async Task<List<User>> GetUsersWhoLikePost(string postUUid) => await userPostRelationRepository.GetUsersWhoLikePostAsync(postUUid);

    public async Task<List<User>> GetUsersWhoVisitPost(string postUUid) => await userPostRelationRepository.GetUsersWhoVisitPostAsync(postUUid);

    public async Task<List<Following>> GetUserFollowings(string username) => await channelUserPostRelationRepository.GetUserFollowingsAsync(username);

    public async Task<List<Follower>> GetUserFollowers(string username) => await channelUserPostRelationRepository.GetUserFollowersAsync(username);

    // Post-related queries
    public async Task<List<Post>> GetPosts() => await postRepository.GetAllPostsAsync();

    public async Task<Post?> GetPost(string uuid) => await postRepository.GetPostAsync(uuid);

    public async Task<List<Post>> GetPostsRegisteredByUser(string username) => await userPostRelationRepository.GetPostsRegisteredByUserAsync(username);

    public async Task<List<Post>> GetPostsLikedByUser(string username) => await userPostRelationRepository.GetPostsLikedByUserAsync(username);

    public async Task<List<PostsVisitedByUserDto>> GetPostsVisitedByUser(string username) => await userPostRelationRepository.GetPostsVisitedByUserAsync(username);

    public async Task<List<Post>> GetChannelPosts(string channelId) => await channelUserPostRelationRepository.GetChannelPostsAsync(channelId);

    public async Task<List<Post>> GetUserPosts(string username) => await channelUserPostRelationRepository.GetUserPostsAsync(username);

    public async Task<List<Post>> GetUserTimelinePosts(string username) => await channelUserPostRelationRepository.GetUserTimelinePostsAsync(username);

    // Comment-related queries
    public async Task<List<NebulaGraphDemo.Dto.Comment>> GetPostComments(string postUUid) => await userPostRelationRepository.GetPostCommentsAsync(postUUid);

    public async Task<List<NebulaGraphDemo.Dto.Comment>> GetUserComments(string username) => await postUserCommentRelationRepository.GetUserCommentsAsync(username);

    public async Task<List<CommentLikeDto>> GetCommentLikes(string commentUUid) => await postUserCommentRelationRepository.GetCommentLikesAsync(commentUUid);

    public async Task<List<CommentsLikedByDto>> GetCommentsLikedBy(string username) => await postUserCommentRelationRepository.GetCommentsLikedByAsync(username);

    public async Task<NebulaGraphDemo.Dto.Comment?> GetComment(string uuid) => await commentRepository.GetCommentAsync(uuid);

    public async Task<List<NebulaGraphDemo.Dto.Comment>> GetAllComments() => await commentRepository.GetAllCommentsAsync();

    // Channel-related queries
    public async Task<NebulaGraphDemo.Dto.Channel?> GetChannel(string channelId) => await channelRepository.GetChannelAsync(channelId);

    public async Task<List<NebulaGraphDemo.Dto.Channel>> GetAllChannels() => await channelRepository.GetAllChannelsAsync();

    public async Task<List<NebulaGraphDemo.Dto.Channel>> SearchChannels(string searchText, string username, int limit) => await channelRepository.SearchChannels(searchText, username, limit);

    public async Task<List<NebulaGraphDemo.Dto.Channel>> GetAllChannelsFullData() => await channelRepository.GetAllChannelsFullDataAsync();

    public async Task<List<TopChannels>> GetTopChannels(int limit) => await channelRepository.GetTopChannelsAsync(limit);

    public async Task<List<Admin>> GetChannelAdmins(string channelId) => await channelRepository.GetChannelAdminsAsync(channelId);
}
