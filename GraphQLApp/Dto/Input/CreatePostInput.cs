using NebulaGraphDemo.Models;

namespace GraphQL.Dto.Input;

public class CreatePostInput
{
    public string IssuerId { get; set; }
    public int IssuerType { get; set; }
    public bool IsShareable { get; set; }
    public bool? IsCommentable { get; set; }
    public bool IsPublic { get; set; }
    public int PostType { get; set; }
    public string RegUser { get; set; }
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
    public string? ParentUuid { get; set; }
    public bool Edited { get; set; } = false;
    public List<Attachment>? Attachments { get; set; } = new();
}