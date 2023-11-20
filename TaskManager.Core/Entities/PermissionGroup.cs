using TaskManager.Core.Core;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Entities
{
    public class PermissionGroup : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Permissions { get; set; } = string.Empty;
        // Relationship
        public ICollection<PermissionRole>? PermissionRoles { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
    }

    public class Permissions
    {
        public PermissionGroupDto? Timeline { get; set; }
        public PermissionGroupDto? Backlog { get; set; }
        public PermissionGroupDto? Board { get; set; }
        public PermissionGroupDto? Project { get; set; }
    }
}
