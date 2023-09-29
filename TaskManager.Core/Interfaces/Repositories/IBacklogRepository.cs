using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IBacklogRepository : IRepository<Backlog>
    {
        Task<Backlog?> GetAsync(Guid id);
        Backlog Add(Backlog backlog);
    }
}
