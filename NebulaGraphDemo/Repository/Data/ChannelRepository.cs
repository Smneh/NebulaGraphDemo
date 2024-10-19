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

    public async Task<NebulaGraphDemo.Dto.Channel?> GetChannelAsync(string channelId)
    {
        // var query = $"LOOKUP ON channel WHERE channel.channelId == '{channelId}' YIELD channel.channelId, channel.title, channel.description;";
        var query = $"MATCH (ch:channel{{channelId:'{channelId}'}}) RETURN ch";
        var result = await _queryExecutor.ExecuteAsync(query);

        var channels = GenericNebulaDataConverter.ConvertToEntityList<NebulaGraphDemo.Dto.Channel>(result);

        return channels.FirstOrDefault();
    }

    public async Task<List<NebulaGraphDemo.Dto.Channel>> GetAllChannelsAsync()
    {
        var query = "MATCH (ch:channel) RETURN ch";
        var result = await _queryExecutor.ExecuteAsync(query);
        var channels = new List<NebulaGraphDemo.Dto.Channel>();
            
        channels = GenericNebulaDataConverter.ConvertToEntityList<NebulaGraphDemo.Dto.Channel>(result);

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

    public async Task<List<Dto.TopChannels>> GetTopChannelsAsync(int limit)
    {
        var query = $@"MATCH (ch:channel)<-[f:follow]-() 
                        RETURN 
                            ch.channel.channelId AS ChannelId, 
                            ch.channel.title AS Title, 
                            ch.channel.profilePictureId AS ProfilePictureId, 
                            COUNT(f) AS FollowerCount 
                        ORDER BY FollowerCount DESC 
                        LIMIT {limit};"; 
        
        var result = await _queryExecutor.ExecuteAsync(query);
        var channels = new List<Dto.TopChannels>();
            
        channels = GenericNebulaDataConverter2.ConvertToEntityList<Dto.TopChannels>(result);

        return channels;
    }
    
    public async Task<List<NebulaGraphDemo.Dto.Channel>> SearchChannels(string searchText, string username, int limit)
    {
        var query = $@"MATCH (ch:channel) 
                        WHERE toLower(ch.channel.channelId) CONTAINS toLower('{searchText}') 
                        OR toLower(ch.channel.title) CONTAINS toLower('{searchText}')
                        OPTIONAL MATCH (u:user {{username: '{username}'}})-[:follow]->(ch) 
                        RETURN 
                        ch.channel.channelId as ChannelId,
                        ch.channel.title as Title,
                        ch.channel.profilePictureId as ProfilePictureId,
                        CASE 
                                WHEN u IS NOT NULL THEN true 
                                ELSE false 
                        END AS IsFollowing
                        LIMIT {limit};"; 
        
        var result = await _queryExecutor.ExecuteAsync(query);
        var channels = new List<NebulaGraphDemo.Dto.Channel>();
            
        channels = GenericNebulaDataConverter2.ConvertToEntityList<NebulaGraphDemo.Dto.Channel>(result);

        return channels;
    }

    public async Task<List<Dto.Admin>> GetChannelAdminsAsync(string channelId)
    {
        var query = @$"MATCH (admin:user)-[:admin_of]->(ch:channel {{channelId:'{channelId}'}}) 
                        RETURN admin.user.username as Username,
                        admin.user.fullname as Fullname,
                        admin.user.profilePictureId as ProfilePictureId;";
        
        var result = await _queryExecutor.ExecuteAsync(query);
        var admins = new List<Dto.Admin>();
            
        admins = GenericNebulaDataConverter2.ConvertToEntityList<Dto.Admin>(result);

        return admins;
    }
}