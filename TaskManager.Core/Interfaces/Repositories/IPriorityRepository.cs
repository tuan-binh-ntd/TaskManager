namespace TaskManager.Core.Interfaces.Repositories;

public interface IPriorityRepository
{
    Task<IReadOnlyCollection<Priority>> GetPrioritiesByProjectIdAsync(Guid projectId);
    Task<Priority> GetMediumPriorityByProjectIdAsync(Guid projectId);
    Task<PaginationResult<PriorityViewModel>> GetPriorityViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput);
    Task<string?> GetNameOfPriorityByIdAsync(Guid priorityId);
    Task<IReadOnlyCollection<PriorityViewModel>> GetPriorityViewModelsByProjectIdAsync(Guid projectId);
    void Insert(Priority priority);
    void Update(Priority priority);
    void Remove(Priority priority);
    Task<Priority?> GetByIdAsync(Guid id);
}
