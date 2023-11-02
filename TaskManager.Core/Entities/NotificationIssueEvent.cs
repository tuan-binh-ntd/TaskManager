using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class NotificationIssueEvent : BaseEntity
    {
        public bool CurrentAssignee { get; set; }
        public bool Reporter { get; set; }
        public bool CurrentUser { get; set; }
        public bool ComponentLead { get; set; }
        public bool AllWatcher { get; set; }
        public string SingleUser { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        //Relationship
        public Guid NotificationId { get; set; }
        public Notification? Notification { get; set; }
        public Guid IssueEventId { get; set; }
        public IssueEvent? IssueEvent { get; set; }
    }
}
