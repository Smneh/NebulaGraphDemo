namespace NebulaGraphDemo.Dto;

public class PostsVisitedByUserDto
{
    // Post fields
    public string Uuid { get; set; }
    public string IssuerId { get; set; }
    public string Attachments { get; set; }
    public string Content { get; set; }
    public DateTime? RegDateTime { get; set; }

    // User who registered the post
    public string PostRegUser { get; set; }
    public string PostRegUserFullname { get; set; }
    public string ProfilePictureId { get; set; }
}