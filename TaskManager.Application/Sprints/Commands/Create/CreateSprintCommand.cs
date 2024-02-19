namespace TaskManager.Application.Sprints.Commands.Create;

public sealed class CreateSprintCommand(
    Guid projectId
    )
    : ICommand<Result<SprintViewModel>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
