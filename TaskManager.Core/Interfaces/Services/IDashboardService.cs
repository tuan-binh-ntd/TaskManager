namespace TaskManager.Core.Interfaces.Services;

public interface IDashboardService
{
    Task<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>> GetIssueOfAssigneeDashboardViewModelAsync(Guid projectId);
    Task<IReadOnlyCollection<IssuesInProjectDashboardViewModel>> GetIssuesInProjectDashboardViewModelAsync(Guid projectId);
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueViewModelsDashboardViewModelAsync(Guid projectId, GetIssuesForAssigneeOrReporterDto issuesForAssigneeOrReporterDto);
}
