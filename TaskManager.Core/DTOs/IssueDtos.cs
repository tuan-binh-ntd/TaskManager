namespace TaskManager.Core.DTOs;

public class CreateIssueDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime? CompleteDate { get; set; }
    public string? Priority { get; set; } = string.Empty;
    public string? Voted { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ParentId { get; set; }
    public Guid IssueTypeId { get; set; }
    public Guid CreatorUserId { get; set; }
}

public class UpdateIssueDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CompleteDate { get; set; }
    public ICollection<Guid>? UserIds { get; set; }
    public string? Voted { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? SprintId { get; set; }
    public Guid? IssueTypeId { get; set; }
    public Guid? BacklogId { get; set; }
    public Guid? AssigneeId { get; set; } = Guid.Empty;
    public Guid? StatusId { get; set; }
    public Guid? PriorityId { get; set; }
    public int? StoryPointEstimate { get; set; } = 0;
    public IReadOnlyCollection<Guid>? VersionIds { get; set; }
    public Guid? ReporterId { get; set; }
    [Required]
    public Guid ModificationUserId { get; set; }
    public IReadOnlyCollection<Guid>? LabelIds { get; set; }
}

public class CreateIssueByNameDto
{
    public string Name { get; set; } = string.Empty;
    public Guid IssueTypeId { get; set; }
    public Guid CreatorUserId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? ParentId { get; set; }
}

public class CreateChildIssueDto
{
    public string Name { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid ParentId { get; set; }
}

public class CreateEpicDto
{
    public string Name { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public Guid ProjectId { get; set; }
}

public class UpdateEpicDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CompleteDate { get; set; }
    public ICollection<Guid>? UserIds { get; set; }
    public string? Voted { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.MinValue;
    public DateTime? DueDate { get; set; } = DateTime.MinValue;
    public Guid? ParentId { get; set; }
    public Guid? AssigneeId { get; set; } = Guid.Empty;
    public Guid? StatusId { get; set; }
    public Guid? PriorityId { get; set; }
    public int? StoryPointEstimate { get; set; } = 0;
    public IReadOnlyCollection<Guid>? VersionIds { get; set; }
    public Guid? ReporterId { get; set; }
    [Required]
    public Guid ModificationUserId { get; set; }
    public IReadOnlyCollection<Guid>? LabelIds { get; set; }
}

public class AddIssueToEpicDto
{
    public Guid IssueId { get; set; }
}

public class DeleteLabelToIssueDto
{
    public Guid LabelId { get; set; }
}
