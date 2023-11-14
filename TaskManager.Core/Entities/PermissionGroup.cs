using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class PermissionGroup : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        // Relationship
        public ICollection<PermissionRole>? PermissionRoles { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
