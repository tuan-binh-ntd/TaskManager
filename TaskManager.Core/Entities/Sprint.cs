namespace TaskManager.Core.Entities;

public class Sprint : BaseEntity
{
    private Sprint(string name, DateTime? startDate, DateTime? endDate, string? goal, Guid projectId)
    {
        Id = Guid.NewGuid();
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Goal = goal;
        IsStart = false;
        IsComplete = false;
        ProjectId = projectId;
    }

    private Sprint()
    {

    }

    public string Name { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Goal { get; set; } = string.Empty;
    public bool IsStart { get; set; } = false;
    public bool IsComplete { get; set; } = false;

    // Relationship
    public Project? Project { get; set; }
    public Guid ProjectId { get; set; }
    public ICollection<Issue>? Issues { get; set; }

    public static Sprint Create(string name, DateTime? startDate, DateTime? endDate, string? goal, Guid projectId)
    {
        return new Sprint(name, startDate, endDate, goal, projectId);
    }

    public void Start()
    {
        IsStart = true;
    }

    public void Complete()
    {
        IsStart = false;
        IsComplete = true;
    }
}
