namespace NebulaGraphDemo.Dto;

public class Following
{
    public string IssuerId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string ProfilePictureId { get; set; } = default!;
    public string Type { get; set; } = default!;
    public DateTime FollowDate { get; set; } = default!;
}