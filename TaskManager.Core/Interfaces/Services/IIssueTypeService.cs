using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IIssueTypeService
    {
        Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypesByProjectId(Guid projectId);
        Task<IssueTypeViewModel> CreateIssueType(Guid projectId, CreateIssueTypeDto createIssueTypeDto);
        Task<IssueTypeViewModel> UpdateIssueType(Guid issueTypeId, UpdateIssueTypeDto updateIssueTypeDto);
    }
}
