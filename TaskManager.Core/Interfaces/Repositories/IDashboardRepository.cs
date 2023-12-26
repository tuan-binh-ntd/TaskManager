using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IDashboardRepository
{
    Task<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>> GetIssueOfAssigneeDashboardViewModel(Guid projectId);
    Task<IReadOnlyCollection<IssuesInProjectDashboardViewModel>> GetIssuesInProjectDashboardViewModelAsync(Guid projectId);
    Task<IReadOnlyCollection<Issue>> GetIssueViewModelsDashboardViewModelAsync(Guid projectId, GetIssuesForAssigneeOrReporterDto getIssuesForAssigneeOrReporterDto);
}
