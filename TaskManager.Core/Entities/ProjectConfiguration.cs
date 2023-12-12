using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class ProjectConfiguration : BaseEntity
{
    public Guid ProjectId { get; set; }
    public int IssueCode { get; set; }
    public int SprintCode { get; set; }
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// Default assginee when create a issue
    /// </summary>
    public Guid? DefaultAssigneeId { get; set; }
    public Guid? DefaultPriorityId { get; set; }
    //Relationship
    public Project? Project { get; set; }
}
