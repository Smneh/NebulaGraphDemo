using NebulaGraphDemo.Models;
using NebulaGraphDemo.Repository.Sessions;
using NebulaGraphDemo.Utilities;
using NebulaNet;

namespace NebulaGraphDemo.Repository.Data;

public class ChannelRepository(NebulaSessionManager sessionManager)
{
    private readonly NebulaQueryExecutor _queryExecutor = new(sessionManager);

    public async Task CreateChannelTagAsync()
    {
        var query = @"CREATE TAG IF NOT EXISTS channel (
                            uuid string,
                            channelId string,
                            title string,
                            creator string,
                            description string,
                            privacyTypeId int,
                            typeId int,
                            regDate int,
                            regTime int,
                            regDateTime datetime,
                            lastUpdateDate datetime,
                            profilePictureId string,
                            wallpaperPictureId string,
                            copyStatus bool,
                            commentStatus bool,
                            pinnedPostId int,
                            postCount int,
                            isPinned bool
                        );";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"channel Tag created, Result: {result}");
    }

    public async Task CreateIndexForChannelAsync()
    {
        //var statement = "CREATE TAG INDEX IF NOT EXISTS idx_user_id ON User(Id);";
        var statement = "CREATE TAG INDEX IF NOT EXISTS channel_index ON channel();";
        await _queryExecutor.ExecuteAsync(statement);
    }

    public async Task CreateChannelAsync(Channel channel)
    {
        var regDateTimeFormatted = channel.RegDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
        var lastUpdateDateFormatted = channel.LastUpdateDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

        var query = @$"INSERT VERTEX channel(uuid, channelId, title, creator, description, 
                    privacyTypeId, typeId, regDate, regTime, regDateTime, 
                    lastUpdateDate, profilePictureId, wallpaperPictureId, 
                    copyStatus, commentStatus, pinnedPostId, postCount, isPinned) VALUES 
                    '{channel.ChannelId}': ('{channel.Uuid}', '{channel.ChannelId}', '{channel.Title}', '{channel.Creator}', '{channel.Description}', 
                    {channel.PrivacyTypeId}, {channel.TypeId}, {channel.RegDate}, {channel.RegTime}, datetime(""{regDateTimeFormatted}""),
                    datetime(""{lastUpdateDateFormatted}""), '{channel.ProfilePictureId}', '{channel.WallpaperPictureId}',
                    {channel.CopyStatus}, {channel.CommentStatus}, {channel.PinnedPostId}, {channel.PostCount}, {channel.IsPinned})";

        var result = await _queryExecutor.ExecuteAsync(query);
        Console.WriteLine($"channel created: {channel.ChannelId}, Result: {result}");
    }

    public async Task<Channel?> GetChannelAsync(string channelId)
    {
        var query = $"MATCH (ch:channel{{channelId:'{channelId}'}}) RETURN ch";
        var result = await _queryExecutor.ExecuteAsync(query);

        var channels = GenericNebulaDataConverter.ConvertToEntityList<Channel>(result);

        return channels.FirstOrDefault();
    }

    public async Task<List<Channel>> GetAllChannelsAsync()
    {
        var query = "MATCH (ch:channel) RETURN ch";
        var result = await _queryExecutor.ExecuteAsync(query);
        var channels = new List<Channel>();
            
        channels = GenericNebulaDataConverter.ConvertToEntityList<Channel>(result);

        return channels;
    }

    public async Task<List<NebulaGraphDemo.Dto.Channel>> GetAllChannelsFullDataAsync()
    {
        var query = @"MATCH (admin:user)-[:admin_of]->(ch:channel) 
                        RETURN ch.channel.channelId AS ChannelId,
                        COLLECT({username: admin.user.username, fullname: admin.user.fullname}) AS Admins;";
        
        var result = await _queryExecutor.ExecuteAsync(query);
        var channels = new List<NebulaGraphDemo.Dto.Channel>();
            
        channels = GenericNebulaDataConverter2.ConvertToEntityList<NebulaGraphDemo.Dto.Channel>(result);

        return channels;
    }

    public async Task<List<NebulaGraphDemo.Dto.Admin>> GetChannelAdminsAsync(string channelId)
    {
        var query = @$"MATCH (admin:user)-[:admin_of]->(ch:channel {{channelId:'{channelId}'}}) 
                        RETURN admin.user.username as Username,
                        admin.user.fullname as Fullname,
                        admin.user.profilePictureId as ProfilePictureId;";
        
        var result = await _queryExecutor.ExecuteAsync(query);
        var admins = new List<NebulaGraphDemo.Dto.Admin>();
            
        admins = GenericNebulaDataConverter2.ConvertToEntityList<NebulaGraphDemo.Dto.Admin>(result);

        return admins;
    }
}