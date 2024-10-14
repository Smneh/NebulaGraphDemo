namespace NebulaGraphDemo.Models;

public class Channel
{
    public string Uuid { get; set; }
    public string ChannelId { get; set; }
    public string Title { get; set; }
    public string Creator { get; set; }
    public string Description { get; set; }
    public int PrivacyTypeId { get; set; }
    public int TypeId { get; set; }
    public long RegDate { get; set; }
    public long RegTime { get; set; }
    public DateTime RegDateTime { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public string ProfilePictureId { get; set; }
    public string WallpaperPictureId { get; set; }
    public bool CopyStatus { get; set; }
    public bool CommentStatus { get; set; }
    public long PinnedPostId { get; set; }
    public int PostCount { get; set; }
    public bool IsPinned { get; set; } = default;
    public bool IsFollowing { get; set; } = default;
}