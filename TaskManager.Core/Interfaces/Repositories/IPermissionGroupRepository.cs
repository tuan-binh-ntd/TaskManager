using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IPermissionGroupRepository : IRepository<PermissionGroup>
    {
        Task<IReadOnlyCollection<PermissionGroupViewModel>> GetByProjectId(Guid projectId);
        Task<PermissionGroup> GetById(Guid id);
        PermissionGroup Add(PermissionGroup permissionGroup);
        void Update(PermissionGroup permissionGroup);
        void Delete(Guid id);
        void AddRange(IReadOnlyCollection<PermissionGroup> permissionGroups);
        Task<PaginationResult<PermissionGroupViewModel>> GetByProjectId(Guid projectId, PaginationInput paginationInput);
        void AddRange(IReadOnlyCollection<UserProject> userProjects);
        Task<IReadOnlyCollection<UserProject>> GetUserProjectsByPermissionGroupId(Guid permissionGroupId);
    }
}
