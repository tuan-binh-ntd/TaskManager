using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IReadOnlyCollection<RoleViewModel>> Gets();
        Task<RoleViewModel> Get(Guid id);
        Task<RoleViewModel> Create(AppRoleDto appRoleDto);
        Task<RoleViewModel> Update(Guid id, AppRoleDto appRoleDto);
        Task<bool> Delete(Guid id);
    }
}
