using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IFilterRepository : IRepository<Filter>
    {
        Task<IReadOnlyCollection<Filter>> GetByUserId(Guid userId);
        Task<Filter> GetById(Guid id);
        Filter Add(Filter filter);
        void Update(Filter filter);
        void Delete(Filter filter);
        void AddRange(IReadOnlyCollection<Filter> filters);
        Task<Filter> GetByName(string name);
        Task<FilterConfiguration?> GetConfigurationOfFilter(Guid id);
    }
}
