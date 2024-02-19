namespace TaskManager.Core.Events.Projects;

public sealed class ProjectCreatedDomainEvent(
    Project project
    )
    : IDomainEvent
{
    public Project Project { get; private set; } = project;
}
