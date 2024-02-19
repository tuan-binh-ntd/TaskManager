namespace TaskManager.Core.Interfaces.Repositories;

public interface IFilterRepository
{
    Task<Filter> GetByNameAsync(string name);
    Task<FilterConfiguration?> GetConfigurationOfFilterAsync(Guid id);
    Task<IReadOnlyCollection<FilterViewModel>> GetFiltersByUserIdAsync(Guid userId);
    void Insert(Filter filter);
    Task<Filter?> GetByIdAsync(Guid id);
    void Remove(Filter filter);
    void InsertRange(IReadOnlyCollection<Filter> filters);
}
