using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IIssueService
    {
        Task<IssueViewModel> CreateIssue(CreateIssueDto createIssueDto, Guid? sprintId = default, Guid? backlogId = default);
        Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto);
        Task<IReadOnlyCollection<IssueViewModel>> GetBySprintId(Guid sprintId);
        Task<IReadOnlyCollection<IssueViewModel>> GetByBacklogId(Guid backlogId);
        Task<IssueViewModel> CreateIssueByName(CreateIssueByNameDto createIssueByNameDto, Guid? sprintId = default, Guid? backlogId = default);
        Task<Guid> DeleteIssue(Guid id);
        Task<ChildIssueViewModel> CreateChildIssue(CreateChildIssueDto createChildIssueDto);
        Task<IssueViewModel> AddEpic(Guid issueId, Guid epicId);
        Task<EpicViewModel> CreateEpic(CreateEpicDto createEpicDto);
        Task<IssueViewModel> GetById(Guid id);
        Task<IReadOnlyCollection<IssueHistoryViewModel>> GetHistoriesByIssueId(Guid issueId);
        Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueId(Guid issueId);
        Task<IssueViewModel> UpdateEpic(Guid id, UpdateEpicDto updateEpicDto);
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByMyOpenIssuesFilter(Guid userId);
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByReportedByMeFilter(Guid userId);
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByAllIssueFilter(Guid userId);
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByOpenIssuesFilter();
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByDoneIssuesFilter();
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByCreatedRecentlyFilter();
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByResolvedRecentlyFilter();
        Task<IReadOnlyCollection<IssueViewModel>> GetIssueByUpdatedRecentlyFilter();
    }
}
