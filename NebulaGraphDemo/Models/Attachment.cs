namespace NebulaGraphDemo.Models;

public class Attachment
{
    public string AttachmentId { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string Extension  { get; set; } = default!;
    public long FileSize { get; set; }
    public string HashCode { get; set; } = default!;
}