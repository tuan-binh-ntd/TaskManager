namespace TaskManager.Core.DTOs;

public class CreateSprintDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Goal { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
}

public class UpdateSprintDto
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Goal { get; set; }
}

public class CompleteSprintDto
{
    /// <summary>
    /// Specific sprint name, New sprint or Backlog string
    /// </summary>
    public string? Option { get; set; } = string.Empty;
    /// <summary>
    /// Field is not null when option is specific sprint name
    /// </summary>
    public Guid? SprintId { get; set; }
}
