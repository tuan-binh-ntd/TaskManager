namespace TaskManager.Application.Statuses.Commands.Delete;

public sealed class DeleteStatusCommand(
    Guid statusId,
    Guid newStatusId
    )
    : ICommand<Result<Guid>>
{
    public Guid StatusId { get; private set; } = statusId;
    public Guid NewStatusId { get; private set; } = newStatusId;
}
