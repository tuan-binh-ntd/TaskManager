namespace TaskManager.Application.Dashboards.Queries.GetIssueViewModelDashboard;

internal class GetIssueViewModelDashboardQueryHandler(
    IDashboardRepository dashboardRepository,
    IIssueRepository issueRepository,
    IMapper mapper
    )
    : IQueryHandler<GetIssueViewModelDashboardQuery, Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    private readonly IDashboardRepository _dashboardRepository = dashboardRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IMapper _mapper = mapper;

    #region Private Methods
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
        await _issueRepository.LoadIssueDetailAsync(issue);
        await _issueRepository.LoadIssueTypeAsync(issue);
        await _issueRepository.LoadStatusAsync(issue);

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

    public async Task<Maybe<IReadOnlyCollection<IssueViewModel>>> Handle(GetIssueViewModelDashboardQuery request, CancellationToken cancellationToken)
    {
        var issues = await _dashboardRepository.GetIssueViewModelsDashboardViewModelAsync(request.ProjectId, request.GetIssuesForAssigneeOrReporterDto);
        return Maybe<IReadOnlyCollection<IssueViewModel>>.From(await ToIssueViewModels(issues));
    }
}
