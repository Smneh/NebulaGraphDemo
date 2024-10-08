namespace NebulaGraphDemo.Dto;

public class CommentLikeDto
{
    public string Username { get; set; } = default!;
    public string Fullname { get; set; } = default!;
    public string? ProfilePictureId { get; set; }
}