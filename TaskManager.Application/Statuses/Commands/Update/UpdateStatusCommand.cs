namespace TaskManager.Application.Statuses.Commands.Update;

public sealed class UpdateStatusCommand(
    Guid statusId,
    UpdateStatusDto updateStatusDto
    )
    : ICommand<Result<StatusViewModel>>
{
    public Guid StatusId { get; private set; } = statusId;
    public UpdateStatusDto UpdateStatusDto { get; private set; } = updateStatusDto;
}
