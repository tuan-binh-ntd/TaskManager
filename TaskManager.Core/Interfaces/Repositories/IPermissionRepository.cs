using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission> GetById(Guid id);
        Permission Add(Permission permission);
        void Update(Permission permission);
        void Delete(Guid id);
        Task<IReadOnlyCollection<Permission>> GetAll();
    }
}
