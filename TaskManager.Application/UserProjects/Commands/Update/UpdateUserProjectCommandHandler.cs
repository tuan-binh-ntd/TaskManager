namespace TaskManager.Application.UserProjects.Commands.Update;

internal sealed class UpdateUserProjectCommandHandler(
    IUserProjectRepository userProjectRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateUserProjectCommand, Result<MemberProjectViewModel>>
{
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<MemberProjectViewModel>> Handle(UpdateUserProjectCommand updateUserProjectCommand, CancellationToken cancellationToken)
    {
        var userProject = await _userProjectRepository.GetByIdAsync(updateUserProjectCommand.UserProjectId);
        if (userProject is null) return Result.Failure<MemberProjectViewModel>(Error.NotFound);
        userProject.ChangePermissionGroup(updateUserProjectCommand.UpdateMemberProjectDto.PermissionGroupId);
        _userProjectRepository.Update(userProject);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(userProject.Adapt<MemberProjectViewModel>());
    }
}
