namespace TaskManager.Core.Entities;

public class UserProject : BaseEntity
{
    private UserProject(string role, Guid permissionGroupId, bool isFavourite, Guid projectId, Guid userId)
    {
        Role = role;
        PermissionGroupId = permissionGroupId;
        IsFavourite = isFavourite;
        ProjectId = projectId;
        UserId = userId;
    }

    private UserProject() { }

    public string Role { get; set; } = string.Empty;
    public Guid PermissionGroupId { get; set; }
    public bool IsFavourite { get; set; }
    // Relationship
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public Project? Project { get; set; }
    public AppUser? User { get; set; }

    public static UserProject Create(string role, Guid permissionGroupId, bool isFavourite, Guid projectId, Guid userId)
    {
        return new UserProject(role, permissionGroupId, isFavourite, projectId, userId);
    }

    public void ChangePermissionGroup(Guid newPermissionGroupId)
    {
        PermissionGroupId = newPermissionGroupId;
    }
}
