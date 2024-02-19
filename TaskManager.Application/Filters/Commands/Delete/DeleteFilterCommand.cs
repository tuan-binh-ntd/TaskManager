namespace TaskManager.Application.Filters.Commands.Delete;

public sealed class DeleteFilterCommand(
    Guid filterId
    )
    : ICommand<Result<Guid>>
{
    public Guid FilterId { get; private set; } = filterId;
}
