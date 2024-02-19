namespace TaskManager.Application.Dashboards.Queries.GetIssueNumOfAssigneeDashboard;

public sealed class GetIssueNumOfAssigneeDashboardQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
