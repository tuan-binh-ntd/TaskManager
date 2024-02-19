namespace TaskManager.Core.Entities;

public class Backlog : BaseEntity
{
    private Backlog(Guid id, string name, Guid projectId)
    {
        Id = id;
        Name = name;
        ProjectId = projectId;
    }

    private Backlog() { }

    public string Name { get; set; } = string.Empty;

    //Relationship
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public ICollection<Issue>? Issues { get; set; }

    public static Backlog Create(string name, Guid projectId)
    {
        return new Backlog(Guid.NewGuid(), name, projectId);
    }
}
