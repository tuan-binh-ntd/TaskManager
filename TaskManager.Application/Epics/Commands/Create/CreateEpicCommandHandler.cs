namespace TaskManager.Application.Epics.Commands.Create;

internal sealed class CreateEpicCommandHandler(
    IProjectConfigurationRepository projectConfigurationRepository,
    UserManager<AppUser> userManager,
    IIssueTypeRepository issueTypeRepository,
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateEpicCommand, Result<EpicViewModel>>
{
    private readonly IProjectConfigurationRepository _projectConfigurationRepository = projectConfigurationRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<EpicViewModel>> Handle(CreateEpicCommand createEpicCommand, CancellationToken cancellationToken)
    {
        var projectConfiguration = await _projectConfigurationRepository
            .GetProjectConfigurationByProjectIdAsync(createEpicCommand.CreateEpicDto.ProjectId);

        if (projectConfiguration is null) return Result.Failure<EpicViewModel>(Error.NotFound);

        int issueIndex = projectConfiguration.IssueCode + 1;

        var creatorUser = await _userManager.FindByIdAsync(createEpicCommand.CreateEpicDto.CreatorUserId.ToString());

        if (creatorUser is null) return Result.Failure<EpicViewModel>(Error.NotFound);

        var issueType = await _issueTypeRepository.GetEpicAsync(createEpicCommand.CreateEpicDto.ProjectId);

        if (issueType is null) return Result.Failure<EpicViewModel>(Error.NotFound);

        var user = User.Create(creatorUser.Id);

        var epic = Issue.CreateEpic(createEpicCommand.CreateEpicDto.Name,
            $"{projectConfiguration.Code}-{issueIndex}",
            issueType.Id,
            createEpicCommand.CreateEpicDto.ProjectId,
            Watcher.CreateFirstWatcher(user));

        epic.IssueDetail = IssueDetail.Create(projectConfiguration.DefaultAssigneeId,
            createEpicCommand.CreateEpicDto.CreatorUserId,
            0,
            epic.Id);

        _issueRepository.Insert(epic);

        projectConfiguration.IssueCode = issueIndex;
        _projectConfigurationRepository.Update(projectConfiguration);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(epic.Adapt<EpicViewModel>());
    }
}
