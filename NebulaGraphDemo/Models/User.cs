namespace NebulaGraphDemo.Models;

public class User
{
    public string Username { get; set; } = default!;
    public string WorkspaceTitle { get; set; } = default!;
    public long WorkspaceId { get; set; } = default!;
    public string Fullname { get; set; } = default!;
    public string? FatherName { get; set; }
    public long LastModifyDate { get; set; }
    public bool? IsMale { get; set; }
    public string? NationalId { get; set; }
    public string? Address { get; set; }
    public string? Tel { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? ProfilePictureId { get; set; }
    public string? WallpaperPictureId { get; set; }
    public string? Bio { get; set; }
    public string? Description { get; set; }
    public string? OrgCode { get; set; }
    public long? OrganizationalUnitId { get; set; }
    public string? OrgInternalPhone { get; set; }
    public long Id { get; set; } = default!;
    public string ProjectColumns { get; set; }
}