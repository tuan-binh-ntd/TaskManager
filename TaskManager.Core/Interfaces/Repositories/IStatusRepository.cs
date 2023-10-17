using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IStatusRepository : IRepository<Status>
    {
        Status Add(Status status);
        void Update(Status status);
        void Delete(Guid id);
        void AddRange(ICollection<Status> statuses);
        Task<IReadOnlyCollection<Status>> GetByProjectId(Guid projectId);
        Task<Status> GetById(Guid id);
    }
}
