namespace TaskManager.Application.UserProjects.Commands.Delete;

public sealed class DeleteUserProjectCommand(
    Guid projectId,
    Guid userProjectId
    )
    : ICommand<Result<Guid>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public Guid UserProjectId { get; private set; } = userProjectId;
}
