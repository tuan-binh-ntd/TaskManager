using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IBacklogRepository : IRepository<Backlog>
    {
        Task<Backlog?> GetAsync(Guid id);
        Backlog Add(Backlog backlog);
        Task<IReadOnlyCollection<IssueViewModel>> GetIssues(Guid backlogId);
        Task<BacklogViewModel> GetBacklog(Guid projectId);
    }
}
