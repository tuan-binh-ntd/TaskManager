using TaskManager.Core.Entities;
using TaskManager.Core.Helper;

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
        Task<Status> GetUnreleasedStatus(Guid projectId);
        Task<PaginationResult<Status>> GetByProjectIdPaging(Guid projectId, PaginationInput paginationInput);
    }
}
