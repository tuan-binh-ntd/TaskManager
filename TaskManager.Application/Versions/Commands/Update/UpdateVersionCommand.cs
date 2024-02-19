namespace TaskManager.Application.Versions.Commands.Update;

public sealed class UpdateVersionCommand(
    Guid versionId,
    UpdateVersionDto updateVersionDto
    )
    : ICommand<Result<VersionViewModel>>
{
    public Guid VersionId { get; private set; } = versionId;
    public UpdateVersionDto UpdateVersionDto { get; private set; } = updateVersionDto;
}
