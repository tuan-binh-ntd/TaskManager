namespace TaskManager.Core.Interfaces.Repositories;

public interface IFilterRepository : IRepository<Filter>
{
    Task<Filter> GetById(Guid id);
    Filter Add(Filter filter);
    void Update(Filter filter);
    void Delete(Filter filter);
    void AddRange(IReadOnlyCollection<Filter> filters);
    Task<Filter> GetByName(string name);
    Task<FilterConfiguration?> GetConfigurationOfFilter(Guid id);
    Task<IReadOnlyCollection<FilterViewModel>> GetFiltersByUserId(Guid userId);
}
