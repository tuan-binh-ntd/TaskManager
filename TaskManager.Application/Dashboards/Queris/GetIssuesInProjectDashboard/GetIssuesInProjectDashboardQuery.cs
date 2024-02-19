namespace TaskManager.Application.Dashboards.Queries.GetIssuesInProjectDashboard;

public sealed class GetIssuesInProjectDashboardQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<IssuesInProjectDashboardViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
