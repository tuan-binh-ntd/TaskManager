using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IStatusRepository : IRepository<Status>
{
    Status Add(Status status);
    void Update(Status status);
    void Delete(Guid id);
    void AddRange(ICollection<Status> statuses);
    Task<IReadOnlyCollection<Status>> GetByProjectId(Guid projectId);
    Task<Status> GetById(Guid id);
    Task<Status> GetUnreleasedStatus(Guid projectId);
    Task<PaginationResult<StatusViewModel>> GetByProjectIdPaging(Guid projectId, PaginationInput paginationInput);
    Task<string?> GetNameOfStatus(Guid statusId);
    Task<bool> CheckStatusBelongDone(Guid statusId);
    Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsAsync(Guid projectId);
    Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelByProjectId(Guid projectId);
    Task<bool> IsReleaseStatus(Guid statusId);
}
