namespace TaskManager.Application.Versions.Commands.Delete;

public sealed class DeleteVersionCommand(
    Guid versionId
    )
    : ICommand<Result<Guid>>
{
    public Guid VersionId { get; private set; } = versionId;
}
