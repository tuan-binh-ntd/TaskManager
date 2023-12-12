using TaskManager.Core.Entities;
using TaskManager.Core.Helper;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IPriorityRepository : IRepository<Priority>
{
    Task<IReadOnlyCollection<Priority>> GetByProjectId(Guid projectId);
    Task<Priority> GetById(Guid id);
    Priority Add(Priority priority);
    void AddRange(IReadOnlyCollection<Priority> priorities);
    void Update(Priority priority);
    void Delete(Guid id);
    Task<Priority> GetMediumByProjectId(Guid projectId);
    Task<PaginationResult<Priority>> GetByProjectId(Guid projectId, PaginationInput paginationInput);
    Task<string?> GetNameOfPriority(Guid priorityId);
}
