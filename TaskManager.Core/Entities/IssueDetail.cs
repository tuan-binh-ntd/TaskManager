using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class IssueDetail : BaseEntity
    {
        public Guid? AssigneeId { get; set; }
        public Guid ReporterId { get; set; }
        public int StoryPointEstimate { get; set; }
        public string Label { get; set; } = string.Empty;

        //Relationship
        public Guid IssueId { get; set; }
        public Issue? Issue { get; set; }
    }
}
