using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class PermissionGroupRepository : IPermissionGroupRepository
    {
        private readonly AppDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public PermissionGroupRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public PermissionGroup Add(PermissionGroup permissionGroup)
        {
            return _context.PermissionGroups.Add(permissionGroup).Entity;
        }

        public void Delete(Guid id)
        {
            var permissionGroup = _context.PermissionGroups.FirstOrDefault(pg => pg.Id == id);
            _context.PermissionGroups.Remove(permissionGroup!);
        }

        public async Task<PermissionGroup> GetById(Guid id)
        {
            var permissionGroup = await _context.PermissionGroups.FirstOrDefaultAsync(pg => pg.Id == id);
            return permissionGroup!;
        }

        public async Task<IReadOnlyCollection<PermissionGroup>> GetByProjectId(Guid projectId)
        {
            var query = from pg in _context.PermissionGroups.Where(pg => pg.ProjectId == projectId)
                        select pg;
            //select new PermissionGroupViewModel
            //{
            //    Id = pg.Id,
            //    Name = pg.Name,
            //    Permissions = 
            //    pg.PermissionRoles!.Select(pr =>
            //    new PermissionViewModel
            //    {
            //        Id = pr.Id,
            //        Name = pr.Permission!.Name,
            //        ParentId = pr.Id,
            //        ViewPermission = pr.ViewPermission,
            //        EditPermission = pr.EditPermission
            //    }).ToList()
            //};
            return await query.ToListAsync();
        }

        public void Update(PermissionGroup permissionGroup)
        {
            _context.Entry(permissionGroup).State = EntityState.Modified;
        }

        public void AddRange(IReadOnlyCollection<PermissionGroup> permissionGroups)
        {
            _context.PermissionGroups.AddRange(permissionGroups);
        }
    }
}
