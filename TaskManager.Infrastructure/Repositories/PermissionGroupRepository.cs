using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
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

        public async Task<IReadOnlyCollection<PermissionGroupViewModel>> GetByProjectId(Guid projectId)
        {
            var query = from pg in _context.PermissionGroups.Where(pg => pg.ProjectId == projectId)
                        select new PermissionGroupViewModel
                        {
                            Id = pg.Id,
                            Name = pg.Name,
                            Permissions = pg.Permissions.FromJson<Permissions>()
                        };
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

        public async Task<PaginationResult<PermissionGroupViewModel>> GetByProjectId(Guid projectId, PaginationInput paginationInput)
        {
            var query = from pg in _context.PermissionGroups.Where(pg => pg.ProjectId == projectId)
                        select new PermissionGroupViewModel
                        {
                            Id = pg.Id,
                            Name = pg.Name,
                            Permissions = pg.Permissions.FromJson<Permissions>()
                        };
            return await query.Pagination(paginationInput);
        }

        public void AddRange(IReadOnlyCollection<UserProject> userProjects)
        {
            _context.UserProjects.AddRange(userProjects);
        }

        public async Task<IReadOnlyCollection<UserProject>> GetUserProjectsByPermissionGroupId(Guid permissionGroupId)
        {
            var userProjects = await _context.UserProjects.Where(up => up.PermissionGroupId == permissionGroupId && up.Role != CoreConstants.LeaderRole).ToListAsync();
            return userProjects.AsReadOnly();
        }
    }
}
