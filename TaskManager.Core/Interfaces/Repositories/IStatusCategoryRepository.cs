using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IStatusCategoryRepository
    {
        IReadOnlyCollection<StatusCategory> Gets();
        Task<StatusCategory?> GetDone();
    }
}
