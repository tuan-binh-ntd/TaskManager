namespace TaskManager.Core.Entities;

public class WorkflowIssueType : BaseEntity
{
    public Guid WorkflowId { get; set; }
    public Guid IssueTypeId { get; set; }
    public Workflow? Workflow { get; set; }
    public IssueType? IssueType { get; set; }
}
