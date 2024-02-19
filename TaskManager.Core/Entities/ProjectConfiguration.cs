namespace TaskManager.Core.Entities;

public class ProjectConfiguration : BaseEntity
{
    private ProjectConfiguration(Guid id, Guid projectId, int issueCode, int sprintCode, string code, Guid? defaultAssigneeId, Guid? defaultPriorityId)
    {
        Id = id;
        ProjectId = projectId;
        IssueCode = issueCode;
        SprintCode = sprintCode;
        Code = code;
        DefaultAssigneeId = defaultAssigneeId;
        DefaultPriorityId = defaultPriorityId;
    }

    private ProjectConfiguration() { }

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

    public static ProjectConfiguration Create(Guid projectId, int issueCode, int sprintCode, string code, Guid? defaultAssigneeId, Guid? defaultPriorityId)
    {
        return new ProjectConfiguration(Guid.NewGuid(), projectId, issueCode, sprintCode, code, defaultAssigneeId, defaultPriorityId);
    }
}
