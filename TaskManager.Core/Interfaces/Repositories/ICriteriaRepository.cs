namespace TaskManager.Core.Interfaces.Repositories;

public interface ICriteriaRepository
{
    Task<IReadOnlyCollection<Criteria>> GetCriteriasAsync();
}
