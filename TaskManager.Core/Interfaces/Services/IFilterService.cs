using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IFilterService
{
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByMyOpenIssuesFilter(Guid userId);
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByReportedByMeFilter(Guid userId);
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByAllIssueFilter(Guid userId);
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByOpenIssuesFilter();
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByDoneIssuesFilter();
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByCreatedRecentlyFilter();
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByResolvedRecentlyFilter();
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByUpdatedRecentlyFilter();
    Task<FilterViewModel> CreateFilter(CreateFilterDto createFilterDto);
    Task<Guid> DeleteFilter(Guid id);
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByFilterConfiguration(Guid id);
}
