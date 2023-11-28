using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class IssueType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Icon { get; set; } = string.Empty;
        public byte Level { get; set; }
        public bool IsMain { get; set; }

        //Relationship
        public ICollection<Issue>? Issues { get; set; }
        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }
        public ICollection<WorkflowIssueType>? WorkflowIssueTypes { get; set; }
    }
}
