namespace TaskManager.Application.UserProjects.Commands.Create;

public sealed class CreateUserProjectCommand(
    AddMemberToProjectDto addMemberToProjectDto
    )
    : ICommand<Result<IReadOnlyCollection<MemberProjectViewModel>>>
{
    public AddMemberToProjectDto AddMemberToProjectDto { get; private set; } = addMemberToProjectDto;
}
