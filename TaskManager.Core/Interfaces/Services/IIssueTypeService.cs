using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IIssueTypeService
    {
        Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypesByProjectId(Guid projectId);
    }
}
