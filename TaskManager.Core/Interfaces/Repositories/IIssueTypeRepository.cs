using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IIssueTypeRepository : IRepository<IssueType>
    {
        Task<IReadOnlyCollection<IssueTypeViewModel>> Gets();
        IssueTypeViewModel Add(IssueType issueType);
        void Update(IssueType issueType);
        void Delete(Guid id);
    }
}
