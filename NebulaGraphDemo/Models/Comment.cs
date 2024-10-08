namespace NebulaGraphDemo.Models;

public class Comment
{
    public string Content { get; set; }
    public int ContentTypeId { get; set; }
    public DateTime RegDateTime { get; set; }
    public int ChildCount { get; set; }
    public long Depth { get; set; }
    public string Uuid { get; set; }
    public string? ParentUuid { get; set; }
    public string? ContentUuid { get; set; }
    public string? CommentContentUuid { get; set; }
}