using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class PermissionRole : BaseEntity
{
    public bool IsActive { get; set; } = true;
    public bool ViewPermission { get; set; } = true;
    public bool EditPermission { get; set; } = true;
    // Relationship
    public Guid PermissionGroupId { get; set; }
    public PermissionGroup? PermissionGroup { get; set; }
    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}
