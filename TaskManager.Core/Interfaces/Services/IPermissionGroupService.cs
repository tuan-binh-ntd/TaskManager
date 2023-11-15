using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IPermissionGroupService
    {
        Task<IReadOnlyCollection<PermissionGroupViewModel>> GetPermissionGroupsByProjectId(Guid projectId);
        Task<PermissionGroupViewModel> Create(CreatePermissionGroupDto createPermissionGroupDto);
        Task<PermissionGroupViewModel> Update(Guid id, UpdatePermissionGroupDto updatePermissionGroupDto);
        Task<Guid> Delete(Guid id);
    }
}
