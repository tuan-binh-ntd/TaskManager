namespace TaskManager.Application.Epics.Commands.Create;

public sealed class CreateEpicCommand(
    CreateEpicDto createEpicDto
    )
    : ICommand<Result<EpicViewModel>>
{
    public CreateEpicDto CreateEpicDto { get; private set; } = createEpicDto;
}
