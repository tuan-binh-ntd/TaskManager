namespace TaskManager.Application.Projects.Queries.GetProjectByCode;

internal class GetProjectByCodeQueryHandler(
    IProjectRepository projectRepository,
    IIssueRepository issueRepository,
    IBacklogRepository backlogRepository,
    ISprintRepository sprintRepository,
    IIssueTypeRepository issueTypeRepository,
    IStatusRepository statusRepository,
    IPriorityRepository priorityRepository,
    IPermissionGroupRepository permissionGroupRepository,
    IStatusCategoryRepository statusCategoryRepository,
    IUserProjectRepository userProjectRepository,
    ILabelRepository labelRepository,
    IProjectConfigurationRepository projectConfigurationRepository,
    IMapper mapper
    )
    : IQueryHandler<GetProjectByCodeQuery, Maybe<ProjectViewModel>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IBacklogRepository _backlogRepository = backlogRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IPermissionGroupRepository _permissionGroupRepository = permissionGroupRepository;
    private readonly IStatusCategoryRepository _statusCategoryRepository = statusCategoryRepository;
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly ILabelRepository _labelRepository = labelRepository;
    private readonly IProjectConfigurationRepository _projectConfigurationRepository = projectConfigurationRepository;
    private readonly IMapper _mapper = mapper;

    #region Private methods
    private async Task<IReadOnlyCollection<IssueViewModel>> ToIssueViewModels(IReadOnlyCollection<Issue> issues)
    {
        var issueViewModels = new List<IssueViewModel>();
        if (issues.Count != 0)
        {
            foreach (var issue in issues)
            {
                var issueViewModel = await ToIssueViewModel(issue);
                issueViewModels.Add(issueViewModel);
            }
        }
        return issueViewModels.AsReadOnly();
    }

    private async Task<IssueViewModel> ToIssueViewModel(Issue issue)
    {
        await _issueRepository.LoadAttachmentsAsync(issue);
        await _issueRepository.LoadIssueDetailAsync(issue);
        await _issueRepository.LoadIssueTypeAsync(issue);
        await _issueRepository.LoadStatusAsync(issue);
        var issueViewModel = _mapper.Map<IssueViewModel>(issue);

        if (issue.IssueDetail is not null)
        {
            var issueDetail = _mapper.Map<IssueDetailViewModel>(issue.IssueDetail);
            issueViewModel.IssueDetail = issueDetail;
        }
        if (issue.Attachments is not null && issue.Attachments.Count != 0)
        {
            var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(issue.Attachments);
            issueViewModel.Attachments = attachments;
        }
        if (issue.IssueType is not null)
        {
            var issueType = _mapper.Map<IssueTypeViewModel>(issue.IssueType);
            issueViewModel.IssueType = issueType;
        }
        if (issue.Status is not null)
        {
            var status = _mapper.Map<StatusViewModel>(issue.Status);
            issueViewModel.Status = status;
        }
        if (issue.ParentId is Guid parentId)
        {
            issueViewModel.ParentName = await _issueRepository.GetParentNameAsync(parentId);
        }
        return issueViewModel;
    }

    private async Task<ProjectViewModel> ToProjectViewModel(Project project, Guid? userId = null)
    {
        var members = await _projectRepository.GetMembersAsync(project.Id);
        var backlog = await _backlogRepository.GetBacklogByProjectIdAsync(project.Id) ?? throw new BacklogNullException();
        var issueForBacklog = await _backlogRepository.GetIssuesByBacklogIdAsync(backlog.Id);
        var issueViewModels = await ToIssueViewModels(issueForBacklog);
        backlog.Issues = issueViewModels.ToList();
        var sprints = await _sprintRepository.GetSprintsByProjectIdAsync(project.Id);
        var issueTypes = await _issueTypeRepository.GetIssueTypesByProjectIdAsync(project.Id);
        var statuses = await _statusRepository.GetStatusViewModelsByProjectIdAsync(project.Id);
        var epics = await _issueRepository.GetEpicsByProjectIdAsync(project.Id);
        var priorities = await _priorityRepository.GetPrioritiesByProjectIdAsync(project.Id);
        var permissionGroups = await _permissionGroupRepository.GetPermissionGroupViewModelsByProjectIdAsync(project.Id);
        var statusCategories = _statusCategoryRepository.GetStatusCategorysAsync();
        if (sprints.Count != 0)
        {
            foreach (var sprint in sprints)
            {
                var issues = await _sprintRepository.GetIssuesBySprintIdAsync(sprint.Id, project.Id);
                issueViewModels = await ToIssueViewModels(issues);
                sprint.Issues = issueViewModels.ToList();
            }
        }
        var projectViewModel = _mapper.Map<ProjectViewModel>(project);
        projectViewModel.Leader = members.Where(m => m.Role == RoleConstants.LeaderRole).SingleOrDefault();
        projectViewModel.Members = members.Where(m => m.Role != RoleConstants.LeaderRole).ToList();
        projectViewModel.Backlog = backlog;
        projectViewModel.Sprints = sprints;
        projectViewModel.IssueTypes = issueTypes;
        projectViewModel.Statuses = statuses.Adapt<IReadOnlyCollection<StatusViewModel>>();
        var epicViewModels = await ToEpicViewModels(epics);
        projectViewModel.Epics = epicViewModels.ToList();
        projectViewModel.Priorities = _mapper.Map<IReadOnlyCollection<PriorityViewModel>>(priorities);
        projectViewModel.StatusCategories = _mapper.Map<IReadOnlyCollection<StatusCategoryViewModel>>(statusCategories);
        if (userId is Guid newUserId)
        {
            projectViewModel.UserPermissionGroup = await _permissionGroupRepository.GetPermissionGroupViewModelByProjectIdAndUserIdAsync(project.Id, newUserId);
            projectViewModel.IsFavourite = await _userProjectRepository.GetIsFavouriteColAsync(project.Id, newUserId);
        }
        projectViewModel.PermissionGroups = permissionGroups;
        var projectConfiguration = _projectConfigurationRepository.GetProjectConfigurationByProjectIdAsync(project.Id);
        projectViewModel.ProjectConfiguration = projectConfiguration.Adapt<ProjectConfigurationViewModel>();
        var labels = await _labelRepository.GetLabelsByProjectIdAsync(project.Id); ;
        projectViewModel.Labels = labels.Adapt<IReadOnlyCollection<LabelViewModel>>();
        return projectViewModel;
    }

    private async Task<IReadOnlyCollection<EpicViewModel>> ToEpicViewModels(IReadOnlyCollection<Issue> epics)
    {
        var epicViewModels = new List<EpicViewModel>();
        if (epics.Count != 0)
        {
            foreach (var issue in epics)
            {
                var epicViewModel = await ToEpicViewModel(issue);
                epicViewModels.Add(epicViewModel);
            }
        }
        return epicViewModels.AsReadOnly();
    }

    private async Task<EpicViewModel> ToEpicViewModel(Issue epic)
    {
        await _issueRepository.LoadEntitiesRelationshipAsync(epic);
        var epicViewModel = _mapper.Map<EpicViewModel>(epic);

        if (epic.IssueDetail is not null)
        {
            var issueDetail = _mapper.Map<IssueDetailViewModel>(epic.IssueDetail);
            epicViewModel.IssueDetail = issueDetail;
        }
        if (epic.IssueHistories is not null && epic.IssueHistories.Count != 0)
        {
            var issueHistories = _mapper.Map<ICollection<IssueHistoryViewModel>>(epic.IssueHistories);
            epicViewModel.IssueHistories = issueHistories;
        }
        if (epic.Comments is not null && epic.Comments.Count != 0)
        {
            var comments = _mapper.Map<ICollection<CommentViewModel>>(epic.Comments);
            epicViewModel.Comments = comments;
        }
        if (epic.Attachments is not null && epic.Attachments.Count != 0)
        {
            var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(epic.Attachments);
            epicViewModel.Attachments = attachments;
        }
        if (epic.IssueType is not null)
        {
            var issueType = _mapper.Map<IssueTypeViewModel>(epic.IssueType);
            epicViewModel.IssueType = issueType;
        }
        if (epic.Status is not null)
        {
            var status = _mapper.Map<StatusViewModel>(epic.Status);
            epicViewModel.Status = status;
        }
        var childIssues = await _issueRepository.GetChildIssuesOfEpicByEpicIdAsync(epic.Id);
        if (childIssues.Count != 0)
        {
            epicViewModel.ChildIssues = await ToIssueViewModels(childIssues);
        }
        return epicViewModel;
    }
    #endregion

    public async Task<Maybe<ProjectViewModel>> Handle(GetProjectByCodeQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProjectByCodeAsync(request.Code);

        if (project is null) return Maybe<ProjectViewModel>.None;

        return Maybe<ProjectViewModel>.From(await ToProjectViewModel(project, request.UserId));
    }
}
