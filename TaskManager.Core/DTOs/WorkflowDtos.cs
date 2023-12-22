namespace TaskManager.Core.DTOs;

public class UpdateWorkflowDto
{
    public IReadOnlyCollection<Guid> IssueTypeIds { get; set; } = new List<Guid>();
}

public class CreateTransitionDto
{
    public string Name { get; set; } = string.Empty;
    public Guid? FromStatusId { get; set; }
    public Guid ToStatusId { get; set; }
}

public class UpdateTransitionDto
{
    public string Name { get; set; } = string.Empty;
    public Guid? FromStatusId { get; set; }
    public Guid ToStatusId { get; set; }
}