namespace TaskManager.Application.Statuses.Commands.Create;

public sealed class CreateStatusCommand(
    CreateStatusDto createStatusDto
    )
    : ICommand<Result<StatusViewModel>>
{
    public CreateStatusDto CreateStatusDto { get; private set; } = createStatusDto;
}
