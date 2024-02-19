namespace TaskManager.Application.UserProjects.Commands.Delete;

internal sealed class DeleteUserProjectCommandHandler(
    IProjectConfigurationRepository projectConfigurationRepository,
    IUserProjectRepository userProjectRepository,
    IIssueDetailRepository issueDetailRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteUserProjectCommand, Result<Guid>>
{
    private readonly IProjectConfigurationRepository _projectConfigurationRepository = projectConfigurationRepository;
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly IIssueDetailRepository _issueDetailRepository = issueDetailRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteUserProjectCommand deleteUserProjectCommand, CancellationToken cancellationToken)
    {
        var defaultAssigneeId = await _projectConfigurationRepository.GetDefaultAssigneeIdByProjectIdAsync(deleteUserProjectCommand.ProjectId);
        var leaderId = await _userProjectRepository.GetLeaderIdByProjectIdAsync(deleteUserProjectCommand.ProjectId);

        var userProject = await _userProjectRepository.GetByIdAsync(deleteUserProjectCommand.UserProjectId) ?? throw new MemberProjectNullException();

        if (userProject.UserId == defaultAssigneeId)
        {
            await _projectConfigurationRepository.UpdateDefaultAssigneeAsync(deleteUserProjectCommand.UserProjectId, leaderId);
            defaultAssigneeId = leaderId;
        }

        await _issueDetailRepository.UpdateOneColumnForIssueDetailAsync(userProject.UserId, defaultAssigneeId, NameColumn.AssigneeId);

        await _issueDetailRepository.UpdateOneColumnForIssueDetailAsync(userProject.UserId, leaderId, NameColumn.ReporterId);

        _userProjectRepository.Remove(userProject);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(deleteUserProjectCommand.UserProjectId);
    }
}
