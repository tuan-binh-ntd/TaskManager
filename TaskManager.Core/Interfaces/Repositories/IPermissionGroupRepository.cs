namespace TaskManager.Core.Interfaces.Repositories;

public interface IPermissionGroupRepository
{
    Task<PaginationResult<PermissionGroupViewModel>> GetPermissionGroupViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput);
    Task<PermissionGroupViewModel> GetPermissionGroupViewModelByProjectIdAndUserIdAsync(Guid projectId, Guid userId);
    Task<IReadOnlyCollection<PermissionGroupViewModel>> GetPermissionGroupViewModelsByProjectIdAsync(Guid projectId);
    Task<Guid> GetDeveloperPermissionGroupIdAsync(Guid projectId);
    Task<IReadOnlyCollection<Guid>> GetPermissionGroupIdByUserId(Guid userId);
    void Insert(PermissionGroup permissionGroup);
    void Update(PermissionGroup permissionGroup);
    void Remove(PermissionGroup permissionGroup);
    Task<PermissionGroup?> GetByIdAsync(Guid id);
    void InsertRange(IReadOnlyCollection<PermissionGroup> permissionGroups);
}
