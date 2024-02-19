namespace TaskManager.Core.Entities;

public class IssueType : BaseEntity
{
    private IssueType()
    {
    }

    private IssueType(Guid id, string name, string? description, string icon, byte level, Guid projectId)
    {
        Id = id;
        Name = name;
        Description = description;
        Icon = icon;
        Level = level;
        ProjectId = projectId;
    }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Icon { get; set; } = string.Empty;
    public byte Level { get; set; }
    public bool IsMain { get; set; }

    //Relationship
    public ICollection<Issue>? Issues { get; set; }
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
    public ICollection<WorkflowIssueType>? WorkflowIssueTypes { get; set; }

    public static IssueType CreateEpicType(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: IssueTypeConstants.EpicName,
                description: IssueTypeConstants.EpicDesc,
                icon: IssueTypeConstants.EpicIcon,
                level: IssueTypeConstants.OneLevel,
                projectId: projectId
            );
    }

    public static IssueType CreateStoryType(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: IssueTypeConstants.StoryName,
                description: IssueTypeConstants.StoryDesc,
                icon: IssueTypeConstants.StoryIcon,
                level: IssueTypeConstants.TwoLevel,
                projectId: projectId
            );
    }

    public static IssueType CreateBugType(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: IssueTypeConstants.BugName,
                description: IssueTypeConstants.BugDesc,
                icon: IssueTypeConstants.BugIcon,
                level: IssueTypeConstants.TwoLevel,
                projectId: projectId
            );
    }

    public static IssueType CreateTaskType(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: IssueTypeConstants.TaskName,
                description: IssueTypeConstants.TaskDesc,
                icon: IssueTypeConstants.TaskIcon,
                level: IssueTypeConstants.TwoLevel,
                projectId: projectId
            );
    }

    public static IssueType CreateSubtaskType(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: IssueTypeConstants.SubtaskName,
                description: IssueTypeConstants.SubtaskDesc,
                icon: IssueTypeConstants.SubtaskIcon,
                level: IssueTypeConstants.ThreeLevel,
                projectId: projectId
            );
    }
}
