using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IIssueHistoryRepository : IRepository<IssueHistory>
    {
        Task<IReadOnlyCollection<IssueHistoryViewModel>> Gets();
        IssueHistoryViewModel Add(IssueHistory issueHistory);
        void Update(IssueHistory issueHistory);
        void Delete(Guid id);
        Task<IReadOnlyCollection<IssueHistory>> GetByIssueId(Guid issueId);
    }
}
