namespace TaskManager.Persistence.Repositories;

public class UserProjectRepository(IDbContext context) : GenericRepository<UserProject>(context)
    , IUserProjectRepository
{
    public async Task<object> GetMembersByProjectIdAsync(Guid projectId, PaginationInput paginationInput)
    {
        if (paginationInput.IsPaging())
        {
            var query = from up in Entity.AsNoTracking().Where(up => up.ProjectId == projectId)
                        join u in DbContext.AppUser.AsNoTracking() on up.UserId equals u.Id
                        select new MemberProjectViewModel
                        {
                            Id = up.Id,
                            Name = u.Name,
                            PermissionGroupId = up.PermissionGroupId,
                            Email = u.Email!
                        };

            return await query.Pagination(paginationInput);
        }

        var members = await (from up in Entity.AsNoTracking().Where(up => up.ProjectId == projectId)
                             join u in DbContext.AppUser.AsNoTracking() on up.UserId equals u.Id
                             select new MemberProjectViewModel
                             {
                                 Id = up.Id,
                                 Name = u.Name,
                                 PermissionGroupId = up.PermissionGroupId,
                                 Email = u.Email!
                             }).ToListAsync();

        return members.AsReadOnly();
    }

    public async Task<MemberProjectViewModel?> GetMemberProjectViewModelByIdAsync(Guid id)
    {
        var query = from up in Entity.AsNoTracking().Where(up => up.Id == id)
                    join u in DbContext.AppUser.AsNoTracking() on up.UserId equals u.Id
                    select new MemberProjectViewModel
                    {
                        Id = up.Id,
                        Name = u.Name,
                        PermissionGroupId = up.PermissionGroupId,
                        Email = u.Email!
                    };

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Guid> GetLeaderIdByProjectIdAsync(Guid projectId)
    {
        var leaderId = await Entity
            .Where(up => up.ProjectId == projectId)
            .Select(up => up.UserId)
            .FirstOrDefaultAsync();

        return leaderId;
    }

    public async Task UpdateIsFavouriteColAsync(Guid projectId, Guid userId, bool isFavourite)
    {
        await Entity
            .Where(up => up.ProjectId == projectId && up.UserId == userId)
            .ExecuteUpdateAsync(up => up.SetProperty(up => up.IsFavourite, isFavourite));
    }

    public async Task<bool> GetIsFavouriteColAsync(Guid projectId, Guid userId)
    {
        return await Entity
            .Where(up => up.ProjectId == projectId && up.UserId == userId)
            .Select(up => up.IsFavourite).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<Guid>> GetProjectIdsByUserIdAsync(Guid userId)
    {
        var projectIds = await Entity
            .AsNoTracking()
            .Where(up => up.UserId == userId)
            .Select(up => up.ProjectId)
            .ToListAsync();

        return projectIds.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<UserProject>> GetUserProjectsByPermissionGroupIdAsync(Guid permissionGroupId)
    {
        var userProjects = await Entity
            .Where(up => up.PermissionGroupId == permissionGroupId && up.Role != RoleConstants.LeaderRole)
            .ToListAsync();

        return userProjects.AsReadOnly();
    }

    public async Task UpdatePermissionGroupIdAsync(Guid oldValue, Guid newValue)
    {
        await Entity
            .Where(i => i.PermissionGroupId == oldValue)
            .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.PermissionGroupId, newValue));
    }

    public async Task<Guid> GetUserIdById(Guid memberId)
    {
        var userId = await Entity.Where(up => up.Id == memberId).Select(up => up.UserId).FirstOrDefaultAsync();
        return userId;
    }
}
