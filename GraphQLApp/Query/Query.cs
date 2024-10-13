using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Data;
using Post = NebulaGraphDemo.Dto.Post;

namespace GraphQL.Query;

public class Query(UserRepository userRepository, PostRepository postRepository)
{
    public async Task<List<User>> GetUsers() => await userRepository.GetAllUsersAsync();

    public async Task<User?> GetUser(string username) => await userRepository.GetUserAsync(username);

    public async Task<List<Post>> GetPosts() => await postRepository.GetAllPostsAsync();

    public async Task<Post?> GetPost(string uuid) => await postRepository.GetPostAsync(uuid);
}
