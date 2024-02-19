namespace TaskManager.Core.Interfaces.Repositories;

public interface IIssueTypeRepository
{
    Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypesByProjectIdAsync(Guid projectId);
    Task<IssueType?> GetSubtaskAsync();
    Task<IssueType?> GetEpicAsync(Guid projectId);
    Task<PaginationResult<IssueTypeViewModel>> GetIssueTypeViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput);
    Task<string?> GetNameOfIssueTypeAsync(Guid issueTypeId);
    Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypeViewModelsByProjectIdAsync(Guid projectId);
    void Insert(IssueType issueType);
    void Update(IssueType issueType);
    void Remove(IssueType issueType);
    Task<IssueType?> GetByIdAsync(Guid id);
}
