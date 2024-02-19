namespace TaskManager.Application.Issues.Commands.Create;
internal sealed class CreateIssueCommandHandler(
    IProjectConfigurationRepository projectConfigurationRepository,
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateIssueCommand, Result<IssueViewModel>>
{
    private readonly IProjectConfigurationRepository _projectConfigurationRepository = projectConfigurationRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IssueViewModel>> Handle(CreateIssueCommand createIssueCommand, CancellationToken cancellationToken)
    {
        var projectConfiguration = await _projectConfigurationRepository.GetProjectConfigurationByProjectIdAsync(createIssueCommand.CreateIssueByNameDto.ProjectId);
        if (projectConfiguration is null) return Result.Failure<IssueViewModel>(Error.NotFound);
        int issueIndex = projectConfiguration.IssueCode + 1;

        var user = User.Create(createIssueCommand.CreateIssueByNameDto.CreatorUserId);

        var issue = Issue.CreateIssue(createIssueCommand.CreateIssueByNameDto.Name,
            $"{projectConfiguration.Code}-{issueIndex}",
            createIssueCommand.CreateIssueByNameDto.IssueTypeId,
            Watcher.CreateFirstWatcher(user),
            projectConfiguration.DefaultPriorityId
            );

        if (createIssueCommand.SprintId is Guid sprintId)
        {
            issue.SprintId = sprintId;
        }
        if (createIssueCommand.BacklogId is Guid backlogId)
        {
            issue.BacklogId = backlogId;
        }

        _issueRepository.Insert(issue);

        projectConfiguration.IssueCode = issueIndex;
        _projectConfigurationRepository.Update(projectConfiguration);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(issue.Adapt<IssueViewModel>());
    }
}
