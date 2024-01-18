namespace TaskManager.Infrastructure.Repositories;

public class UserProjectRepository : IUserProjectRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public UserProjectRepository(
        AppDbContext context
        )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public UserProject Add(UserProject userProject)
    {
        return _context.UserProjects.Add(userProject).Entity;
    }

    public void Update(UserProject userProject)
    {
        _context.Entry(userProject).State = EntityState.Modified;
    }

    public UserProject? Get(Guid projectId, Guid userId)
    {
        return _context.UserProjects.AsNoTracking().Where(e => e.ProjectId == projectId && e.UserId == userId).FirstOrDefault();
    }

    public async Task<object> GetMemberProjects(Guid projectId, PaginationInput paginationInput)
    {
        if (paginationInput.IsPaging())
        {
            var query = from up in _context.UserProjects.AsNoTracking().Where(up => up.ProjectId == projectId)
                        join u in _context.Users.AsNoTracking() on up.UserId equals u.Id
                        select new MemberProjectViewModel
                        {
                            Id = up.Id,
                            Name = u.Name,
                            PermissionGroupId = up.PermissionGroupId,
                            Email = u.Email!
                        };

            return await query.Pagination(paginationInput);
        }

        var members = await (from up in _context.UserProjects.AsNoTracking().Where(up => up.ProjectId == projectId)
                             join u in _context.Users.AsNoTracking() on up.UserId equals u.Id
                             select new MemberProjectViewModel
                             {
                                 Id = up.Id,
                                 Name = u.Name,
                                 PermissionGroupId = up.PermissionGroupId,
                                 Email = u.Email!
                             }).ToListAsync();

        return members.AsReadOnly();
    }

    public async Task<UserProject?> GetMember(Guid id)
    {
        var member = await _context.UserProjects.Where(up => up.Id == id).FirstOrDefaultAsync();
        return member;
    }

    public async Task<MemberProjectViewModel?> GetMemberProject(Guid id)
    {
        var query = from up in _context.UserProjects.AsNoTracking().Where(up => up.Id == id)
                    join u in _context.Users.AsNoTracking() on up.UserId equals u.Id
                    select new MemberProjectViewModel
                    {
                        Id = up.Id,
                        Name = u.Name,
                        PermissionGroupId = up.PermissionGroupId,
                        Email = u.Email!
                    };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<Guid>> GetByUserId(Guid userId)
    {
        var permissionGroupIds = await (from up in _context.UserProjects.AsNoTracking().Where(up => up.UserId == userId)
                                        join u in _context.Users.AsNoTracking() on up.UserId equals u.Id
                                        select up.PermissionGroupId).ToListAsync();

        return permissionGroupIds.AsReadOnly();
    }

    public void Delete(UserProject userProject)
    {
        _context.UserProjects.Remove(userProject);
    }

    public async Task<UserProject?> GetById(Guid id)
    {
        var userProject = await _context.UserProjects.Where(up => up.Id == id).FirstOrDefaultAsync();
        return userProject;
    }

    public async Task<Guid> GetLeaderIdByProjectId(Guid projectId)
    {
        var leaderId = await _context.UserProjects.Where(up => up.ProjectId == projectId).Select(up => up.UserId).FirstOrDefaultAsync();
        return leaderId;
    }

    public async Task UpdateIsFavouriteCol(Guid projectId, Guid userId, bool isFavourite)
    {
        await _context.UserProjects
            .Where(up => up.ProjectId == projectId && up.UserId == userId)
            .ExecuteUpdateAsync(up => up.SetProperty(up => up.IsFavourite, isFavourite));
    }

    public async Task<bool> GetIsFavouriteCol(Guid projectId, Guid userId)
    {
        return await _context.UserProjects
            .Where(up => up.ProjectId == projectId && up.UserId == userId)
            .Select(up => up.IsFavourite).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<Guid>> GetProjectIdsByUserId(Guid userId)
    {
        var projectIds = await _context.UserProjects.AsNoTracking().Where(up => up.UserId == userId).Select(up => up.ProjectId).ToListAsync();
        return projectIds.AsReadOnly();
    }
}
