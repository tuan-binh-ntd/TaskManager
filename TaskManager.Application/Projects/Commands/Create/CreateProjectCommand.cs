namespace TaskManager.Application.Projects.Commands.Create;

public sealed class CreateProjectCommand(
    Guid userId,
    CreateProjectDto createProjectDto
    )
    : ICommand<Result<ProjectViewModel>>
{
    public Guid UserId { get; private set; } = userId;
    public CreateProjectDto CreateProjectDto { get; private set; } = createProjectDto;
}
