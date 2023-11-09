using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class PermissionRole : BaseEntity
    {
        public bool IsActive { get; set; } = true;
        // Relationship
        public Guid RoleId { get; set; }
        public AppRole? Role { get; set; }
        public Guid PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
