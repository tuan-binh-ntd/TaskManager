using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class UserProject : BaseEntity
    {
        public string Role { get; set; } = string.Empty;
        // Relationship
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public Project? Project { get; set; }
        public AppUser? User { get; set; }
    }
}
