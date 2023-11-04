using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface ICriteriaRepository : IRepository<Criteria>
    {
        Task<IReadOnlyCollection<Criteria>> Gets();
    }
}
