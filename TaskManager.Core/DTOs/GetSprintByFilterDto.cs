namespace TaskManager.Core.DTOs;

public class GetSprintByFilterDto
{
    public IReadOnlyCollection<Guid>? EpicIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid>? LabelIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid>? IssueTypeIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid>? SprintIds { get; set; } = new List<Guid>();
    public string? SearchKey { get; set; } = string.Empty;
}
