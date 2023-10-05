using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IIssueService
    {
        Task<IssueViewModel> CreateIssue(CreateIssueDto createIssueDto);
        Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto);
        //Task<IReadOnlyCollection<IssueDetailViewModel>> GetBySprintId(Guid sprintId);
    }
}
