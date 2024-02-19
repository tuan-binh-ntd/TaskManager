namespace TaskManager.Application.Sprints.Commands.Start;

public sealed class StartSprintCommand(
    Guid sprintId,
    UpdateSprintDto updateSprintDto
    )
    : ICommand<Result<SprintViewModel>>
{
    public Guid SprintId { get; private set; } = sprintId;
    public UpdateSprintDto UpdateSprintDto { get; private set; } = updateSprintDto;
}
