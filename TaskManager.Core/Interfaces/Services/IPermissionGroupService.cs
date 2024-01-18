namespace TaskManager.Core.Interfaces.Services;

public interface IPermissionGroupService
{
    Task<object> GetPermissionGroupsByProjectId(Guid projectId, PaginationInput paginationInput);
    Task<PermissionGroupViewModel> Create(CreatePermissionGroupDto createPermissionGroupDto);
    Task<PermissionGroupViewModel> Update(Guid id, UpdatePermissionGroupDto updatePermissionGroupDto, Guid projectId);
    Task<Guid> Delete(Guid id, Guid? newPermissionGroupId, Guid projectId);
}
