namespace TaskManager.Application.Projects.Commands.Update;

public sealed class UpdateProjectCommand(
    Guid projectId,
    Guid userId,
    UpdateProjectDto updateProjectDto
    )
    : ICommand<Result<ProjectViewModel>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public Guid UserId { get; private set; } = userId;
    public UpdateProjectDto UpdateProjectDto { get; private set; } = updateProjectDto;
}
