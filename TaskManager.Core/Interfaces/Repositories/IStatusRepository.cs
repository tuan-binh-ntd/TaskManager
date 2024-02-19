namespace TaskManager.Core.Interfaces.Repositories;

public interface IStatusRepository
{
    Task<Status> GetUnreleasedStatusByProjectIdAsync(Guid projectId);
    Task<PaginationResult<StatusViewModel>> GetStatusViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput);
    Task<string?> GetNameOfStatusAsync(Guid statusId);
    Task<bool> CheckStatusBelongDoneAsync(Guid statusId);
    Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsOfIssueByProjectIdAsync(Guid projectId);
    Task<bool> IsReleaseStatusAsync(Guid statusId);
    Task<Status?> GetByIdAsync(Guid id);
    void Insert(Status status);
    void Update(Status status);
    void Remove(Status status);
    Task<IReadOnlyCollection<Status>> GetStatusesByProjectIdAsync(Guid projectId);
}
