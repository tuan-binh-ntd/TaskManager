namespace TaskManager.Persistence.Repositories;

public class PermissionGroupRepository(IDbContext context) : GenericRepository<PermissionGroup>(context)
    , IPermissionGroupRepository
{
    public async Task<PaginationResult<PermissionGroupViewModel>> GetPermissionGroupViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput)
    {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var query = from pg in Entity.Where(pg => pg.ProjectId == projectId)
                    join up in DbContext.Set<UserProject>() on pg.Id equals up.PermissionGroupId into upj
                    from uplj in upj.DefaultIfEmpty()
                    group new { pg, uplj } by new { pg.Id, pg.Name, pg.Permissions, IssueId = uplj.PermissionGroupId, pg.IsMain } into g
                    select new PermissionGroupViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Permissions = g.Key.Permissions.FromJson<Permissions>(),
                        MemberCount = g.Count(g => g.uplj.PermissionGroupId != null),
                        IsMain = g.Key.IsMain
                    };
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        return await query.Pagination(paginationInput);
    }

    public async Task<PermissionGroupViewModel> GetPermissionGroupViewModelByProjectIdAndUserIdAsync(Guid projectId, Guid userId)
    {
        var permissionGroupId = await DbContext.Set<UserProject>()
            .AsNoTracking()
            .Where(up => up.ProjectId == projectId && up.UserId == userId)
            .Select(up => up.PermissionGroupId)
            .FirstOrDefaultAsync();

        var permissionGroup = await Entity
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

    public async Task<IReadOnlyCollection<PermissionGroupViewModel>> GetPermissionGroupViewModelsByProjectIdAsync(Guid projectId)
    {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var query = from pg in Entity.Where(pg => pg.ProjectId == projectId)
                    join up in DbContext.Set<UserProject>() on pg.Id equals up.PermissionGroupId into upj
                    from uplj in upj.DefaultIfEmpty()
                    group new { pg, uplj } by new { pg.Id, pg.Name, pg.Permissions, IssueId = uplj.PermissionGroupId, pg.IsMain } into g
                    select new PermissionGroupViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Permissions = g.Key.Permissions.FromJson<Permissions>(),
                        MemberCount = g.Count(g => g.uplj.PermissionGroupId != null),
                        IsMain = g.Key.IsMain
                    };
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

        return await query.ToListAsync();
    }



    public async Task<Guid> GetDeveloperPermissionGroupIdAsync(Guid projectId)
    {
        var developerId = await Entity
            .AsNoTracking()
            .Where(pg => pg.ProjectId == projectId && pg.Name == PermissionGroupConstants.DeveloperName)
            .Select(pg => pg.Id)
            .FirstOrDefaultAsync();

        return developerId;
    }

    public async Task<IReadOnlyCollection<Guid>> GetPermissionGroupIdByUserId(Guid userId)
    {
        var permissionGroupIds = await (from up in DbContext.Set<UserProject>().AsNoTracking().Where(up => up.UserId == userId)
                                        join u in DbContext.AppUser.AsNoTracking() on up.UserId equals u.Id
                                        select up.PermissionGroupId).ToListAsync();

        return permissionGroupIds.AsReadOnly();
    }
}
