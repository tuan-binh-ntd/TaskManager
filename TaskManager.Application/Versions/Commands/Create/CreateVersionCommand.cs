namespace TaskManager.Application.Versions.Commands.Create;

public sealed class CreateVersionCommand(
    CreateVersionDto createVersionDto
    )
    : ICommand<Result<VersionViewModel>>
{
    public CreateVersionDto CreateVersionDto { get; private set; } = createVersionDto;
}
