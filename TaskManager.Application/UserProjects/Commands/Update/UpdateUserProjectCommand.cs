namespace TaskManager.Application.UserProjects.Commands.Update;

public sealed class UpdateUserProjectCommand(
    Guid userProjectId,
    UpdateMemberProjectDto updateMemberProjectDto
    )
    : ICommand<Result<MemberProjectViewModel>>
{
    public Guid UserProjectId { get; private set; } = userProjectId;
    public UpdateMemberProjectDto UpdateMemberProjectDto { get; private set; } = updateMemberProjectDto;
}
