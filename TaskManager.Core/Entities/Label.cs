using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Label : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        // Relationship
        public ICollection<LabelIssue>? LabelIssues { get; set; }
    }
}
