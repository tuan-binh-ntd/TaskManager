namespace TaskManager.Application.Priorities.Commands.Delete;

public sealed class DeletePriorityCommand(
    Guid priorityId,
    Guid newPriorityId
    )
    : ICommand<Result<Guid>>
{
    public Guid PriorityId { get; private set; } = priorityId;
    public Guid NewPriorityId { get; private set; } = newPriorityId;
}
