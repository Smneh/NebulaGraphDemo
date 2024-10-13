namespace NebulaGraphDemo.Dto;

public class Follower
{
    public string Username { get; set; } = default!;
    public string Fullname { get; set; } = default!;
    public string ProfilePictureId { get; set; } = default!;
    public DateTime FollowDate { get; set; } = default!;
}