using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;
using NebulaNet;

namespace NebulaGraphDemo.Repository.Data;

public class UserRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);

    public async Task CreateUserTagAsync()
    {
        var query = @"CREATE TAG IF NOT EXISTS user (
                            Username string,
                            WorkspaceTitle string,
                            WorkspaceId int,
                            Fullname string,
                            FatherName string,
                            LastModifyDate timestamp,
                            IsMale bool,
                            NationalId string,
                            Address string,
                            Tel string,
                            Email string,
                            Mobile string,
                            ProfilePictureId string,
                            WallpaperPictureId string,
                            Bio string,
                            Description string,
                            OrgCode string,
                            OrganizationalUnitId int,
                            OrgInternalPhone string,
                            Id int,
                            ProjectColumns string
                        );";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"user Tag created, Result: {result}");
    }

    public async Task CreateIndexForUserAsync()
    {
        //var statement = "CREATE TAG INDEX IF NOT EXISTS idx_user_id ON User(Id);";
        var statement = "CREATE TAG INDEX IF NOT EXISTS user_index ON user();";
        await _queryExecutor.ExecuteAsync(statement);
    }

    public async Task CreateUserAsync(User user)
    {
        var query = $"INSERT VERTEX user(Username, WorkspaceTitle, WorkspaceId, Fullname, FatherName, " +
                    $"LastModifyDate, IsMale, NationalId, Address, Tel, Email, Mobile, " +
                    $"ProfilePictureId, WallpaperPictureId, Bio, Description, OrgCode, " +
                    $"OrganizationalUnitId, OrgInternalPhone, Id, ProjectColumns) VALUES " +
                    $"'{user.Username}': ('{user.Username}', '{user.WorkspaceTitle}', {user.WorkspaceId}, " +
                    $"'{user.Fullname}', '{user.FatherName}', {user.LastModifyDate}, " +
                    $"{(user.IsMale.HasValue ? user.IsMale.Value.ToString().ToLower() : "NULL")}, " +
                    $"'{user.NationalId}', '{user.Address}', '{user.Tel}', '{user.Email}', '{user.Mobile}', " +
                    $"'{user.ProfilePictureId}', '{user.WallpaperPictureId}', '{user.Bio}', " +
                    $"'{user.Description}', '{user.OrgCode}', {user.OrganizationalUnitId}, " +
                    $"'{user.OrgInternalPhone}', {user.Id}, '{user.ProjectColumns}')";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"user created: {user.Fullname}, Result: {result}");
    }

    public async Task<User?> GetUserAsync(string username)
    {
        var query = $"MATCH (u:user{{Username:'{username}'}}) RETURN u";
        var result = await _queryExecutor.ExecuteAsync(query);

        var users = GenericNebulaDataConverter.ConvertToEntityList<User>(result);

        return users.FirstOrDefault();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var query = "MATCH (u:user) RETURN u";
        var result = await _queryExecutor.ExecuteAsync(query);
        var users = new List<User>();
            
        users = GenericNebulaDataConverter.ConvertToEntityList<User>(result);

        return users;
    }
}