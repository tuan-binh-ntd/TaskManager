using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

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
                    join up in _context.UserProjects on pg.Id equals up.PermissionGroupId into upj
                    from uplj in upj.DefaultIfEmpty()
                    group new { pg, uplj } by new { pg.Id, pg.Name, pg.Permissions, IssueId = uplj.PermissionGroupId } into g
                    select new PermissionGroupViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Permissions = g.Key.Permissions.FromJson<Permissions>(),
                        MemberCount = g.Count(g => g.uplj.PermissionGroupId != null)
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

    public async Task<PermissionGroupViewModel> GetPermissionGroupViewModelById(Guid projectId, Guid userId)
    {
        var permissionGroupId = await _context.UserProjects.AsNoTracking().Where(up => up.ProjectId == projectId && up.UserId == userId).Select(up => up.PermissionGroupId).FirstOrDefaultAsync();

        var permissionGroup = await _context.PermissionGroups
            .AsNoTracking()
            .Select(pg => new PermissionGroupViewModel
            {
                Id = pg.Id,
                Name = pg.Name,
                Permissions = pg.Permissions.FromJson<Permissions>()
            })
            .FirstOrDefaultAsync(pg => pg.Id == permissionGroupId);
        return permissionGroup!;
    }

    public async Task<IReadOnlyCollection<PermissionGroupViewModel>> GetPermissionGroupViewModelsByProjectId(Guid projectId)
    {
        var query = from pg in _context.PermissionGroups.Where(pg => pg.ProjectId == projectId)
                    join up in _context.UserProjects on pg.Id equals up.PermissionGroupId into upj
                    from uplj in upj.DefaultIfEmpty()
                    group new { pg, uplj } by new { pg.Id, pg.Name, pg.Permissions, IssueId = uplj.PermissionGroupId } into g
                    select new PermissionGroupViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Permissions = g.Key.Permissions.FromJson<Permissions>(),
                        MemberCount = g.Count(g => g.uplj.PermissionGroupId != null)
                    };
        return await query.ToListAsync();
    }

    public async Task UpdatePermissionGroupId(Guid oldValue, Guid newValue)
    {
        await _context.UserProjects
                   .Where(i => i.PermissionGroupId == oldValue)
                   .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.PermissionGroupId, newValue));
    }
}
