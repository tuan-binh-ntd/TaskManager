using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Notification : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        // Relationship
        public Guid CreatorUserId { get; set; }
        public AppUser? User { get; set; }
        public ICollection<NotificationIssueEvent>? NotificationIssueEvents { get; set; }
        public Project? Project { get; set; }
    }
}
