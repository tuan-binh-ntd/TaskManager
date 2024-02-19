namespace TaskManager.Core.Entities;

public class Priority : BaseEntity
{
    private Priority()
    {
    }

    private Priority(Guid id, string name, string description, string color, string icon, Guid projectId)
    {
        Id = id;
        Name = name;
        Description = description;
        Color = color;
        Icon = icon;
        ProjectId = projectId;
    }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsMain { get; set; }

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }

    public static Priority CreateLowestPriority(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: PriorityConstants.LowestName,
                description: PriorityConstants.LowestDesc,
                color: PriorityConstants.LowestColor,
                icon: PriorityConstants.LowestIcon,
                projectId: projectId
            );
    }

    public static Priority CreateLowPriority(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: PriorityConstants.LowName,
                description: PriorityConstants.LowDesc,
                color: PriorityConstants.LowColor,
                icon: PriorityConstants.LowIcon,
                projectId: projectId
            );
    }

    public static Priority CreateMediumPriority(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: PriorityConstants.MediumName,
                description: PriorityConstants.MediumDesc,
                color: PriorityConstants.MediumColor,
                icon: PriorityConstants.MediumColor,
                projectId: projectId
            );
    }

    public static Priority CreateHighPriority(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: PriorityConstants.HighName,
                description: PriorityConstants.HighDesc,
                color: PriorityConstants.HighColor,
                icon: PriorityConstants.HighIcon,
                projectId: projectId
            );
    }

    public static Priority CreateHighestPriority(Guid projectId)
    {
        return new
            (
                id: Guid.NewGuid(),
                name: PriorityConstants.HighestName,
                description: PriorityConstants.HighestDesc,
                color: PriorityConstants.HighestColor,
                icon: PriorityConstants.HighestIcon,
                projectId: projectId
            );
    }
}
