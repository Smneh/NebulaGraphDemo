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
            // Set other fields from input
        };
        await userRepository.CreateUserAsync(user);
        return user;
    }

    public async Task<Post> CreatePost(CreatePostInput input)
    {
        var post = new Post
        {
            IssuerId = input.IssuerId,
            Content = input.Content,
            RegUser = input.RegUser,
            // Set other fields from input
        };
        await postRepository.CreatePostAsync(post.ToDto());
        return post;
    }
}
