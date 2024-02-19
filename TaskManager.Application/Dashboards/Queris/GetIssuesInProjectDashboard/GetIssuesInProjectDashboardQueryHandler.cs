namespace TaskManager.Application.Dashboards.Queries.GetIssuesInProjectDashboard;

internal sealed class GetIssuesInProjectDashboardQueryHandler(
    IDashboardRepository dashboardRepository
    )
    : IQueryHandler<GetIssuesInProjectDashboardQuery, Maybe<IReadOnlyCollection<IssuesInProjectDashboardViewModel>>>
{
    private readonly IDashboardRepository _dashboardRepository = dashboardRepository;

    public async Task<Maybe<IReadOnlyCollection<IssuesInProjectDashboardViewModel>>> Handle(GetIssuesInProjectDashboardQuery request, CancellationToken cancellationToken)
    {
        var issuesInProject = await _dashboardRepository.GetIssuesInProjectDashboardViewModelAsync(request.ProjectId);
        return Maybe<IReadOnlyCollection<IssuesInProjectDashboardViewModel>>.From(issuesInProject);
    }
}
