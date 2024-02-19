namespace TaskManager.Core.Interfaces.Repositories;

public interface IStatusCategoryRepository
{
    Task<IReadOnlyCollection<StatusCategory>> GetStatusCategorysAsync();
    Task<StatusCategory?> GetDoneStatusCategoryAsync();
    Task<IReadOnlyCollection<StatusCategory>> GetStatusCategorysForStatusOfIssueAsync();
}
