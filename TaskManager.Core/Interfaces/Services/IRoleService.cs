using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IRoleService
{
    Task<IReadOnlyCollection<RoleViewModel>> GetByProjectId(Guid projectId);
    Task<RoleViewModel> Get(Guid id);
    Task<RoleViewModel> Create(CreateAppRoleDto appRoleDto);
    Task<RoleViewModel> Update(Guid id, CreateAppRoleDto appRoleDto);
    Task<Guid> Delete(Guid id);
}
