using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IIssueTypeRepository : IRepository<IssueType>
    {
        Task<IReadOnlyCollection<IssueTypeViewModel>> Gets();
        IssueTypeViewModel Add(IssueType issueType);
        void Update(IssueType issueType);
        void Delete(Guid id);
        Task<IReadOnlyCollection<IssueTypeViewModel>> GetsByProjectId(Guid projectId);
        Task<IssueType> Get(Guid id);
        Task<IssueType> GetSubtask();
        Task<IssueType> GetEpic();
        Task<PaginationResult<IssueTypeViewModel>> GetsByProjectIdPaging(Guid projectId, PaginationInput paginationInput);
    }
}
