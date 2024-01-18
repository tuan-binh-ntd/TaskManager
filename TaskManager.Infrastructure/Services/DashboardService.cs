namespace TaskManager.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly IIssueDetailRepository _issueDetailRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IMapper _mapper;

    public DashboardService(
        IDashboardRepository dashboardRepository,
        IIssueDetailRepository issueDetailRepository,
        IIssueRepository issueRepository,
        IMapper mapper
        )
    {
        _dashboardRepository = dashboardRepository;
        _issueDetailRepository = issueDetailRepository;
        _issueRepository = issueRepository;
        _mapper = mapper;
    }

    #region Private Methods
    private async Task<IReadOnlyCollection<IssueViewModel>> ToIssueViewModels(IReadOnlyCollection<Issue> issues)
    {
        var issueViewModels = new List<IssueViewModel>();
        if (issues.Any())
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
        await _issueRepository.LoadIssueDetail(issue);
        await _issueRepository.LoadIssueType(issue);
        await _issueRepository.LoadStatus(issue);

        var issueViewModel = _mapper.Map<IssueViewModel>(issue);

        if (issue.IssueDetail is not null)
        {
            var issueDetail = _mapper.Map<IssueDetailViewModel>(issue.IssueDetail);
            issueViewModel.IssueDetail = issueDetail;
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
        return issueViewModel;
    }
    #endregion

    public async Task<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>> GetIssueOfAssigneeDashboardViewModelAsync(Guid projectId)
    {
        var issueOfAssignees = await _dashboardRepository.GetIssueOfAssigneeDashboardViewModel(projectId);
        return issueOfAssignees;
    }

    public async Task<IReadOnlyCollection<IssuesInProjectDashboardViewModel>> GetIssuesInProjectDashboardViewModelAsync(Guid projectId)
    {
        var issuesInProject = await _dashboardRepository.GetIssuesInProjectDashboardViewModelAsync(projectId);
        return issuesInProject;
    }

    public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueViewModelsDashboardViewModelAsync(Guid projectId, GetIssuesForAssigneeOrReporterDto issuesForAssigneeOrReporterDto)
    {
        var issues = await _dashboardRepository.GetIssueViewModelsDashboardViewModelAsync(projectId, issuesForAssigneeOrReporterDto);
        return await ToIssueViewModels(issues);
    }
}
