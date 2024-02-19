namespace TaskManager.Application.Sprints.Commands.Complete;

public sealed class CompleteSprintCommand(
    Guid projectId,
    Guid sprintId,
    CompleteSprintDto completeSprintDto
    )
    : ICommand<Result<SprintViewModel>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public Guid SprintId { get; private set; } = sprintId;
    public CompleteSprintDto CompleteSprintDto { get; private set; } = completeSprintDto;
}
