namespace TaskManager.Application.Sprints.Commands.Delete;

public sealed class DeleteSprintCommand(
    Guid sprintId
    )
    : ICommand<Result<Guid>>
{
    public Guid SprintId { get; private set; } = sprintId;
}
