using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public PermissionRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Permission Add(Permission permission)
        {
            return _context.Permissions.Add(permission).Entity;
        }

        public void Delete(Guid id)
        {
            var permission = _context.Permissions.Where(e => e.Id == id).FirstOrDefault();
            _context.Permissions.Remove(permission!);
        }

        public async Task<Permission> GetById(Guid id)
        {
            var permission = await _context.Permissions.Where(e => e.Id == id).FirstOrDefaultAsync();
            return permission!;
        }

        public void Update(Permission permission)
        {
            _context.Entry(permission).State = EntityState.Modified;
        }
    }
}
