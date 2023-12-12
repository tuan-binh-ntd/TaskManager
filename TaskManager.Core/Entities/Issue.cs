using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class Issue : BaseEntity
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.None)]
    //public override Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime? CompleteDate { get; set; }
    public Watcher? Watcher { get; set; }
    public string? Voted { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    //Relationship
    public Guid? ParentId { get; set; }
    public Guid? SprintId { get; set; }
    public Sprint? Sprint { get; set; }
    public Guid IssueTypeId { get; set; }
    public IssueType? IssueType { get; set; }
    public Guid? BacklogId { get; set; }
    public Backlog? Backlog { get; set; }
    public ICollection<IssueHistory>? IssueHistories { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Attachment>? Attachments { get; set; }
    public IssueDetail? IssueDetail { get; set; }
    public Guid? StatusId { get; set; }
    public Status? Status { get; set; }
    public Guid? PriorityId { get; set; }
    public Priority? Priority { get; set; }
    public ICollection<VersionIssue>? VersionIssues { get; set; }
    public ICollection<LabelIssue>? LabelIssues { get; set; }
    public Guid? ProjectId { get; set; }
}

public class Watcher
{
    public List<User>? Users { get; set; } = new();
}

public class User
{
    public Guid Identity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class AssigneeFromTo
{
    public Guid? From { get; set; }
    public Guid? To { get; set; }
}

public class ReporterFromTo
{
    public Guid From { get; set; }
    public Guid To { get; set; }
}
