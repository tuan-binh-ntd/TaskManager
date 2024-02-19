namespace TaskManager.Application.Projects.Commands.Delete;

public sealed class DeleteProjectCommand(
    Guid projectId
    )
    : ICommand<Result<Guid>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
