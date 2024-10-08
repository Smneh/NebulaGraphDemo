namespace NebulaGraphDemo.Dto;

public class CommentsLikedByDto
{
    // Comment fields
    public string Content { get; set; }
    public int? ContentTypeId { get; set; }
    public DateTime? RegDateTime { get; set; }
    public int? ChildCount { get; set; }
    public long? Depth { get; set; }
    public string Uuid { get; set; }
    public string ParentUuid { get; set; }
    public string ContentUuid { get; set; }
    public string CommentContentUuid { get; set; }

    // Post fields
    public string PostContent { get; set; }
    public string PostAttachments { get; set; }

    // User who made the comment
    public string CommentRegUser { get; set; }
    public string CommentRegUserFullname { get; set; }
    public string CommentRegUserProfilePictureId { get; set; }

    // User who made the post
    public string PostRegUser { get; set; }
    public string PostRegUserFullname { get; set; }
    public string PostRegUserProfilePictureId { get; set; }
}