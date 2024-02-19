namespace TaskManager.Application.Sprints.Commands.Update;

public sealed class UpdateSprintCommand(
    Guid sprintId,
    UpdateSprintDto updateSprintDto
    )
    : ICommand<Result<SprintViewModel>>
{
    public Guid SprintId { get; private set; } = sprintId;
    public UpdateSprintDto UpdateSprintDto { get; private set; } = updateSprintDto;
}
