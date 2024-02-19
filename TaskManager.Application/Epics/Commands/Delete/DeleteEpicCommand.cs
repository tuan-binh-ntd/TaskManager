namespace TaskManager.Application.Epics.Commands.Delete;

public sealed class DeleteEpicCommand(
    Guid epicId,
    Guid userId
    )
    : ICommand<Result<Guid>>
{
    public Guid EpicId { get; private set; } = epicId;
    public Guid UserId { get; private set; } = userId;
}
