using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission> GetById(Guid id);
        Permission Add(Permission permission);
        void Update(Permission permission);
        void Delete(Permission permission);
        Task<IReadOnlyCollection<Permission>> GetAll();
        Task LoadPermissionRoles(Permission permission);
    }
}
