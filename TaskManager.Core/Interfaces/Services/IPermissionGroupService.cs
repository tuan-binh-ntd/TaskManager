using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IPermissionGroupService
    {
        Task<object> GetPermissionGroupsByProjectId(Guid projectId, PaginationInput paginationInput);
        Task<PermissionGroupViewModel> Create(CreatePermissionGroupDto createPermissionGroupDto);
        Task<PermissionGroupViewModel> Update(Guid id, UpdatePermissionGroupDto updatePermissionGroupDto);
        Task<Guid> Delete(Guid id);
    }
}
