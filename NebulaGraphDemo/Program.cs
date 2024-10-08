using NebulaGraphDemo.Extensions;
using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Data;
using NebulaGraphDemo.Repository.Sessions;

namespace NebulaGraphDemo;

class Program
{
    private static UserRepository _userRepository;
    private static PostRepository _postRepository;
    private static UserPostRelationRepository _userPostRelationRepository;
    private static CommentRepository _commentRepository;
    private static PostUserCommentRelationRepository _postUserCommentRelationRepository;

    static async Task Main(string[] args)
    {
        using var sessionManager = new NebulaSessionManager("192.168.111.141", 9669);
        try
        {
            // Check if the space exists before using it
            await sessionManager.EnsureSpaceExistsAsync("SamanehSpace");

            await sessionManager.UseSpaceAsync("SamanehSpace");

            _userRepository = new UserRepository(sessionManager);
            _postRepository = new PostRepository(sessionManager);
            _userPostRelationRepository = new UserPostRelationRepository(sessionManager);
            _commentRepository = new CommentRepository(sessionManager);
            _postUserCommentRelationRepository = new PostUserCommentRelationRepository(sessionManager);

            // await CreateSchema();
            // await Task.Delay(10000); // 10 seconds

            var (username1, username2) = await UsersSection();
            var (uuid1, uuid2) = await PostsSection();

            await RegisterPostRelationSection(username1, uuid1);
            await RegisterPostRelationSection(username2, uuid2);
            var (cmUuid1, cmUuid2) = await CommentSection();

            await RegisterUserPostCommentRelationSection(username1, uuid1, cmUuid1);
            await RegisterUserPostCommentRelationSection(username2, uuid2, cmUuid2);

            await RegisterUserLikeCommentRelationSection(username1, cmUuid2);
            await RegisterUserLikeCommentRelationSection(username2, cmUuid1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static async Task CreateSchema()
    {
        await _userRepository.CreateUserTagAsync();
        await _userRepository.CreateIndexForUserAsync();
        
        await _postRepository.CreatePostTagAsync();
        await _postRepository.CreateIndexForPostAsync();
        
        await _userPostRelationRepository.CreateRegisterEdgeAsync();
        await _userPostRelationRepository.CreateLikeEdgeAsync();
        await _userPostRelationRepository.CreateVisitEdgeAsync();
        //await _userPostRelationRepository.CreateCommentEdgeAsync();
        
        await _commentRepository.CreateCommentTagAsync();
        await _commentRepository.CreateIndexForCommentAsync();
        
        await _postUserCommentRelationRepository.CreateCommentEdgeAsync();
        await _postUserCommentRelationRepository.CreateBelongsToEdgeAsync();
    }

    private static async Task<(string, string)> UsersSection()
    {
        var (username1, username2) = await CreateUsers();

        var user = await _userRepository.GetUserAsync(username1);
        if (user != null)
        {
            Console.WriteLine($"Retrieved User: {user.Fullname}, Email: {user.Email}, Workspace: {user.WorkspaceTitle}");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
        
        // Retrieve all users
        var users = await _userRepository.GetAllUsersAsync();
        Console.WriteLine("All Users:");
        foreach (var u in users)
        {
            Console.WriteLine($"ID: {u.Id}, Username: {u.Username}, Fullname: {u.Fullname}, Email: {u.Email}");
        }

        return (username1, username2);
    }

    private static async Task<(string, string)> PostsSection()
    {
        var (uuid1, uuid2) = await CreatePosts();

        var post = await _postRepository.GetPostAsync(uuid1);
        if (post != null)
        {
            var postEntity = post.ToEntity();
            Console.WriteLine($"Retrieved Post IssuerId: {postEntity.IssuerId}, Content: {postEntity.Content}");
        }
        else
        {
            Console.WriteLine("Post not found.");
        }
        
        var posts = await _postRepository.GetAllPostsAsync();
        Console.WriteLine("All Posts:");
        foreach (var p in posts)
        {
            var postEntity = p.ToEntity();
            Console.WriteLine($"IssuerId: {postEntity.IssuerId}, Content: {postEntity.Content}");
        }

        return (uuid1, uuid2);
    }

    private static async Task<(string, string)> CreateUsers()
    {
        var user1 = new User
        {
            Username = "smneh",
            WorkspaceTitle = "Development",
            WorkspaceId = 1,
            Fullname = "smneh am",
            FatherName = "",
            LastModifyDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            IsMale = true,
            NationalId = "123456789",
            Address = "123 Main St",
            Tel = "123-456-7890",
            Email = "smneh@example.com",
            Mobile = "555-555-5555",
            ProfilePictureId = "smnehpic",
            WallpaperPictureId = "smnehpic",
            Bio = "Software Developer",
            Description = "Loves coding and coffee.",
            OrgCode = "DEV",
            OrganizationalUnitId = 101,
            OrgInternalPhone = "123-456",
            Id = 3,
            ProjectColumns = "Column1"
        };

        var user2 = new User
        {
            Username = "hamid",
            WorkspaceTitle = "Design",
            WorkspaceId = 2,
            Fullname = "hamid b",
            FatherName = null,
            LastModifyDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            IsMale = true,
            NationalId = "987654321",
            Address = "456 Main St",
            Tel = "987-654-3210",
            Email = "hamid@example.com",
            Mobile = "444-444-4444",
            ProfilePictureId = "hamidpic",
            WallpaperPictureId = "hamidpic",
            Bio = "Graphic Designer",
            Description = "Passionate about design.",
            OrgCode = "DES",
            OrganizationalUnitId = 102,
            OrgInternalPhone = "987-654",
            Id = 4,
            ProjectColumns = "Column3"
        };

        await _userRepository.CreateUserAsync(user1);

        await _userRepository.CreateUserAsync(user2);

        return (user1.Username, user2.Username);
    }

    private static async Task<(string, string)> CreatePosts()
    {
        var post1 = new Post
        {
            IssuerId = "smneh",
            IssuerType = 1,
            RegDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            RegTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            IsShareable = true,
            IsCommentable = true,
            IsPublic = true,
            PostType = 1,
            RegUser = "smneh",
            UniqueId = Guid.NewGuid().ToString(),
            RegDateTime = DateTime.Now,
            LikeCount = 0,
            CommentCount = 0,
            ViewCount = 0,
            Content = "This is the smneh post content.",
            IssuerPostId = 1,
            IsSurvey = false,
            PostTypeId = 1,
            ParentIssuerType = "",
            SurveyId = 0,
            ShareCount = 0,
            Uuid = Guid.NewGuid().ToString(),
            ContentUuid = null,
            ParentUuid = null,
            Edited = false,
            EditDateTime = DateTime.Now,
            Attachments =
            [
                new Attachment
                {
                    AttachmentId = "1",
                    FileName = "attachment1.jpg",
                    Extension = ".jpg",
                    FileSize = 1024,
                    HashCode = "123456789"
                },
                new Attachment
                {
                    AttachmentId = "2",
                    FileName = "attachment2.png",
                    Extension = ".png",
                    FileSize = 2048,
                    HashCode = "9876954321"
                }
            ]
        };

        var post2 = new Post
        {
            IssuerId = "hamid",
            IssuerType = 1,
            RegDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            RegTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            IsShareable = false,
            IsCommentable = true,
            IsPublic = false,
            PostType = 1,
            RegUser = "hamid",
            UniqueId = Guid.NewGuid().ToString(),
            RegDateTime = DateTime.Now,
            LikeCount = 5,
            CommentCount = 2,
            ViewCount = 10,
            Content = "This is the hamid post content.",
            IssuerPostId = 2,
            IsSurvey = false,
            PostTypeId = 2,
            ParentIssuerType = "",
            SurveyId = 0,
            ShareCount = 1,
            Uuid = Guid.NewGuid().ToString(),
            ContentUuid = null,
            ParentUuid = null,
            Edited = false,
            EditDateTime = DateTime.Now,
            Attachments =
            [
                new Attachment
                {
                    AttachmentId = "1",
                    FileName = "attach1.jpg",
                    Extension = ".jpg",
                    FileSize = 1024,
                    HashCode = "19451"
                },
                new Attachment
                {
                    AttachmentId = "2",
                    FileName = "attach2.png",
                    Extension = ".png",
                    FileSize = 2048,
                    HashCode = "9821"
                }
            ]
        };

        var post1Dto = post1.ToDto();
        await _postRepository.CreatePostAsync(post1Dto);

        var post2Dto = post2.ToDto();
        await _postRepository.CreatePostAsync(post2Dto);

        return (post1.Uuid, post2.Uuid);
    }

    private static async Task RegisterPostRelationSection(string username, string uuid)
    {
        await _userPostRelationRepository.AddPostRegisterEdgeAsync(username, uuid);

        await _userPostRelationRepository.AddPostLikeEdgeAsync(username, uuid);

        await _userPostRelationRepository.AddPostVisitEdgeAsync(username, uuid);

        // var comment1 = new Comment
        // {
        //     Content = $"cm1 {username}",
        //     ContentTypeId = 1,
        //     RegDateTime = DateTime.Now,
        //     ChildCount = 0,
        //     Depth = 0,
        //     Uuid = Guid.NewGuid().ToString(),
        //     ParentUuid = "-",
        //     ContentUuid = Guid.NewGuid().ToString(),
        //     CommentContentUuid = Guid.NewGuid().ToString(),
        // };
        //
        // var comment2 = new Comment
        // {
        //     Content = $"cm2 {username}",
        //     ContentTypeId = 1,
        //     RegDateTime = DateTime.Now,
        //     ChildCount = 0,
        //     Depth = 0,
        //     Uuid = Guid.NewGuid().ToString(),
        //     ParentUuid = "-",
        //     ContentUuid = Guid.NewGuid().ToString(),
        //     CommentContentUuid = Guid.NewGuid().ToString(),
        // };        
        //
        // var comment3 = new Comment
        // {
        //     Content = $"cm3 {username}",
        //     ContentTypeId = 1,
        //     RegDateTime = DateTime.Now,
        //     ChildCount = 0,
        //     Depth = 0,
        //     Uuid = Guid.NewGuid().ToString(),
        //     ParentUuid = "-",
        //     ContentUuid = Guid.NewGuid().ToString(),
        //     CommentContentUuid = Guid.NewGuid().ToString(),
        // };
        //
        // await _userPostRelationRepository.AddPostCommentEdgeAsync(username, uuid, comment1);
        // await _userPostRelationRepository.AddPostCommentEdgeAsync(username, uuid, comment2);
        // await _userPostRelationRepository.AddPostCommentEdgeAsync(username, uuid, comment3);

        var posts = await _userPostRelationRepository.GetPostsRegisteredByUserAsync(username);
        
        var likers = await _userPostRelationRepository.GetUsersWhoLikePostAsync(uuid);
        var likedPosts = await _userPostRelationRepository.GetPostsLikedByUserAsync(username);
        
        var visitors = await _userPostRelationRepository.GetUsersWhoVisitPostAsync(uuid);
        var postsVisitedBy = await _userPostRelationRepository.GetPostsVisitedByUserAsync(username);
        
        //var comments = await _userPostRelationRepository.GetPostCommentsAsync(uuid);

        Console.WriteLine($"Posts Registered By {username}");
        foreach (var p in posts)
        {
            var postEntity = p.ToEntity();
            Console.WriteLine($"IssuerId: {postEntity.IssuerId}, Content: {postEntity.Content}");
        }
        
        Console.WriteLine($"Users Who Like Post {uuid}");
        foreach (var u in likers)
        {
            Console.WriteLine($"ID: {u.Id}, Username: {u.Username}, Fullname: {u.Fullname}, Email: {u.Email}");
        }
        
        Console.WriteLine($"Users Who Visit Post {uuid}");
        foreach (var u in visitors)
        {
            Console.WriteLine($"ID: {u.Id}, Username: {u.Username}, Fullname: {u.Fullname}, Email: {u.Email}");
        }
        
        Console.WriteLine($"Posts Liked By {username}");
        foreach (var p in likedPosts)
        {
            var postEntity = p.ToEntity();
            Console.WriteLine($"IssuerId: {postEntity.IssuerId}, Content: {postEntity.Content}, RegDateTime: {postEntity.RegDateTime}");
        }
        
        Console.WriteLine($"Posts Visited By {username}");
        foreach (var p in postsVisitedBy)
        {
            Console.WriteLine($"IssuerId: {p.IssuerId}, Content: {p.Content}, RegDateTime: {p.RegDateTime}");
        }
        // Console.WriteLine($"Comments of post {uuid}");
        // foreach (var cm in comments)
        // {
        //     Console.WriteLine($"Content: {cm.Content}, RegUser: {cm.RegUser}, RegDateTime: {cm.RegDateTime}");
        // }
    }

    private static async Task<(string, string)> CommentSection()
    {
        var (uuid1, uuid2) = await CreateComments();

        return (uuid1, uuid2);
    }

    private static async Task<(string, string)> CreateComments()
    {
        var comment1 = new Models.Comment
        {
            Content = $"cm1",
            ContentTypeId = 1,
            RegDateTime = DateTime.Now,
            ChildCount = 0,
            Depth = 0,
            Uuid = Guid.NewGuid().ToString(),
            ParentUuid = "-",
            ContentUuid = Guid.NewGuid().ToString(),
            CommentContentUuid = Guid.NewGuid().ToString(),
        };

        var comment2 = new Models.Comment
        {
            Content = $"cm2",
            ContentTypeId = 1,
            RegDateTime = DateTime.Now,
            ChildCount = 0,
            Depth = 0,
            Uuid = Guid.NewGuid().ToString(),
            ParentUuid = "-",
            ContentUuid = Guid.NewGuid().ToString(),
            CommentContentUuid = Guid.NewGuid().ToString(),
        };

        await _commentRepository.CreateCommentAsync(comment1);

        await _commentRepository.CreateCommentAsync(comment2);

        return (comment1.Uuid, comment2.Uuid);
    }

    private static async Task RegisterUserPostCommentRelationSection(string username, string postUuid, string commentUuid)
    {
        await _postUserCommentRelationRepository.AddCommentBelongsToEdgeAsync(commentUuid, postUuid);

        await _postUserCommentRelationRepository.AddUserCommentedEdgeAsync(username, commentUuid);

        var postComments = await _postUserCommentRelationRepository.GetPostCommentsAsync(postUuid);
        var userComments = await _postUserCommentRelationRepository.GetUserCommentsAsync(username);

        Console.WriteLine($"Comments of post {postUuid}");
        foreach (var c in postComments)
        {
            Console.WriteLine($"RegUser: {c.RegUser}, Content: {c.Content}, RegDateTime: {c.RegDateTime}");
        }

        Console.WriteLine($"Comments of user {username}");
        foreach (var c in userComments)
        {
            Console.WriteLine($"RegUser: {c.RegUser}, Content: {c.Content}, RegDateTime: {c.RegDateTime}");
        }
    }

    private static async Task RegisterUserLikeCommentRelationSection(string username, string commentUuid)
    {
        await _postUserCommentRelationRepository.AddCommentLikeEdgeAsync(username, commentUuid);
        
        var commentLikes = await _postUserCommentRelationRepository.GetCommentLikesAsync(commentUuid);
        var commentsLikedBy = await _postUserCommentRelationRepository.GetCommentsLikedByAsync(username);

        Console.WriteLine($"Like of comment {commentUuid}");
        foreach (var like in commentLikes)
        {
            Console.WriteLine($"username: {like.Username}, Fullname: {like.Fullname}, ProfilePictureId: {like.ProfilePictureId}");
        }

        Console.WriteLine($"Comments Liked By {username}");
        foreach (var c in commentsLikedBy)
        {
            Console.WriteLine($"CommentRegUser: {c.CommentRegUser}, CommentContent: {c.Content}, RegDateTime: {c.RegDateTime}, " +
                              $"PostRegUser: {c.PostRegUser}, PostContent {c.PostContent}");
        }
    }
}