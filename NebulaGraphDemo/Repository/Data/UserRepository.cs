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
                            username string,
                            workspaceTitle string,
                            workspaceId int,
                            fullname string,
                            fatherName string,
                            lastModifyDate timestamp,
                            isMale bool,
                            nationalId string,
                            address string,
                            tel string,
                            email string,
                            mobile string,
                            profilePictureId string,
                            wallpaperPictureId string,
                            bio string,
                            description string,
                            orgCode string,
                            organizationalUnitId int,
                            orgInternalPhone string,
                            id int,
                            projectColumns string
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
        var query = $"INSERT VERTEX user(username, workspaceTitle, workspaceId, fullname, fatherName, " +
                    $"lastModifyDate, isMale, nationalId, address, tel, email, mobile, " +
                    $"profilePictureId, wallpaperPictureId, bio, description, orgCode, " +
                    $"organizationalUnitId, orgInternalPhone, id, projectColumns) VALUES " +
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
    
    public async Task UpdateUserAsync(User user)
    {
        var query = $@"UPDATE VERTEX ON user '{user.Username}' 
                        SET fullname = '{user.Fullname}', 
                            profilePictureId = '{user.ProfilePictureId}', 
                            lastModifyDate = {user.LastModifyDate}
                            ;";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"User updated: {user.Username}, Result: {result}");
    }
    
    public async Task<User?> GetUserAsync(string username)
    {
        var query = $"MATCH (u:user{{username:'{username}'}}) RETURN u";
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