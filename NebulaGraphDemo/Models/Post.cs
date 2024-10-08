using NebulaGraphDemo.Enums;

namespace NebulaGraphDemo.Models;

public class Post
{
    public string IssuerId { get; set; }
    public int IssuerType { get; set; }
    public long RegDate { get; set; }
    public long RegTime { get; set; }
    public bool IsShareable { get; set; }
    public bool? IsCommentable { get; set; }
    public bool IsPublic { get; set; }
    public int PostType { get; set; }
    public string RegUser { get; set; }
    public string UniqueId { get; set; }
    public DateTime RegDateTime { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }
    public string Content { get; set; }
    public int IssuerPostId { get; set; }
    public bool IsSurvey { get; set; }
    public long PostTypeId { get; set; }
    public string ParentIssuerType { get; set; }
    public long SurveyId { get; set; }
    public int ShareCount { get; set; }
    public string Uuid { get; set; } = default!;
    public string? ContentUuid { get; set; }
    public string? ParentUuid { get; set; }
    public bool Edited { get; set; } = false;
    public DateTime EditDateTime { get; set; }
    public List<Attachment>? Attachments { get; set; } = new();
}