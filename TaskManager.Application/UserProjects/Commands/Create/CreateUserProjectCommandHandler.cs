namespace TaskManager.Application.UserProjects.Commands.Create;

internal sealed class CreateUserProjectCommandHandler(
    IUserProjectRepository userProjectRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateUserProjectCommand, Result<IReadOnlyCollection<MemberProjectViewModel>>>
{
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyCollection<MemberProjectViewModel>>> Handle(CreateUserProjectCommand createUserProjectCommand, CancellationToken cancellationToken)
    {
        if (createUserProjectCommand.AddMemberToProjectDto.UserIds is not null
            && createUserProjectCommand.AddMemberToProjectDto.UserIds.Count != 0)
        {
            var userProjects = createUserProjectCommand.AddMemberToProjectDto.UserIds
                .Select(userId => UserProject.Create(RoleConstants.MemberRole,
                    createUserProjectCommand.AddMemberToProjectDto.PermissionGroupId,
                    false, createUserProjectCommand.AddMemberToProjectDto.ProjectId,
                    userId))
                .ToList();
            _userProjectRepository.InsertRange(userProjects);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(userProjects.Adapt<IReadOnlyCollection<MemberProjectViewModel>>());
        }

        return Result.Failure<IReadOnlyCollection<MemberProjectViewModel>>(UserProjectDomainErrors.CanNotInsert);
    }
}
