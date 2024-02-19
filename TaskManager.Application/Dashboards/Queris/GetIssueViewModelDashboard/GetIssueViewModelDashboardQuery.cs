namespace TaskManager.Application.Dashboards.Queries.GetIssueViewModelDashboard;

public class GetIssueViewModelDashboardQuery(
    Guid projectId,
    GetIssuesForAssigneeOrReporterDto getIssuesForAssigneeOrReporterDto
    )
    : IQuery<Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public GetIssuesForAssigneeOrReporterDto GetIssuesForAssigneeOrReporterDto { get; private set; } = getIssuesForAssigneeOrReporterDto;
}
