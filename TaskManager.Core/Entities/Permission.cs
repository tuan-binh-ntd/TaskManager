using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }

        // Relationship
        public ICollection<PermissionRole>? PermissionRoles { get; set; }
    }
}
