using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IPermissionService
{
    Task<PermissionViewModel> CreatePermission(CreatePermissionDto createPermissionDto);
    Task<PermissionViewModel> UpdatePermission(Guid id, UpdatePermissionDto updatePermissionDto);
    Task<Guid> DeletePermission(Guid id);
    Task<IReadOnlyCollection<PermissionViewModel>> GetPermissionViewModels();
}
