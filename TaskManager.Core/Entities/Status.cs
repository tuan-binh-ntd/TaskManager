using TaskManager.Core.Events.Statuses;

namespace TaskManager.Core.Entities;

public class Status : BaseEntity
{
    private Status(Guid id, string name, string description, int ordering, bool isMain, bool allowAnyStatus, Guid? projectId, Guid statusCategoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        Ordering = ordering;
        IsMain = isMain;
        AllowAnyStatus = allowAnyStatus;
        ProjectId = projectId;
        StatusCategoryId = statusCategoryId;
    }

    private Status() { }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool IsMain { get; set; }
    /// <summary>
    /// Allow issues in any status to move to this one
    /// </summary>
    public bool AllowAnyStatus { get; set; }
    //Relationship
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
    public Guid StatusCategoryId { get; set; }
    public StatusCategory? StatusCategory { get; set; }
    public ICollection<Issue>? Issues { get; set; }

    public static Status Create(string name, string description, int ordering, bool isMain, bool allowAnyStatus, Guid? projectId, Guid statusCategoryId)
    {
        return new Status(Guid.NewGuid(), name, description, ordering, isMain, allowAnyStatus, projectId, statusCategoryId);
    }

    public void StatusDeleted(Guid newStatusId)
    {
        AddDomainEvent(new StatusDeletedDomainEvent(Id, newStatusId));
    }
}
