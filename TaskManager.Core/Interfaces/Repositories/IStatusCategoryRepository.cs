using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IStatusCategoryRepository : IRepository<StatusCategory>
{
    IReadOnlyCollection<StatusCategory> Gets();
    Task<StatusCategory?> GetDone();
    Task<IReadOnlyCollection<StatusCategory>> GetForStatus();
}
