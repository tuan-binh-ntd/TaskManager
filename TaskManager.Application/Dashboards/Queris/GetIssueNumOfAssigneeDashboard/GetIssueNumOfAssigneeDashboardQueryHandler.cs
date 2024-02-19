namespace TaskManager.Application.Dashboards.Queries.GetIssueNumOfAssigneeDashboard;

internal class GetIssueNumOfAssigneeDashboardQueryHandler(
    IDashboardRepository dashboardRepository
    )
    : IQueryHandler<GetIssueNumOfAssigneeDashboardQuery, Maybe<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>>>
{
    private readonly IDashboardRepository _dashboardRepository = dashboardRepository;

    public async Task<Maybe<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>>> Handle(GetIssueNumOfAssigneeDashboardQuery request, CancellationToken cancellationToken)
    {
        var issueOfAssignees = await _dashboardRepository.GetIssueOfAssigneeDashboardViewModel(request.ProjectId);
        return Maybe<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>>.From(issueOfAssignees);
    }
}
