namespace TaskManager.Core.Entities;

public class Backlog : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    //Relationship
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public ICollection<Issue>? Issues { get; set; }
}
