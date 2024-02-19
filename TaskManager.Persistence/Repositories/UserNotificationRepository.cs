namespace TaskManager.Persistence.Repositories;

public class UserNotificationRepository(IDbContext context) : GenericRepository<UserNotification>(context)
    , IUserNotificationRepository
{
    public async Task<IReadOnlyCollection<UserNotificationViewModel>> GetUserNotificationViewModelsByUserIdAsync(Guid userId)
    {
        var userNotificationViewModels = await (from un in Entity.AsNoTracking().Where(un => un.UserId == userId)
                                                join u in DbContext.AppUser on un.CreatorUserId equals u.Id
                                                join i in DbContext.Set<Issue>() on un.IssueId equals i.Id
                                                join it in DbContext.Set<IssueType>() on i.IssueTypeId equals it.Id
                                                join s in DbContext.Set<Status>() on i.StatusId equals s.Id

                                                join sp in DbContext.Set<Sprint>() on i.SprintId equals sp.Id into spj
                                                from sprint in spj.DefaultIfEmpty()
                                                join spp in DbContext.Set<Project>() on sprint.ProjectId equals spp.Id into sppj
                                                from pro in sppj.DefaultIfEmpty()

                                                join b in DbContext.Set<Backlog>() on i.BacklogId equals b.Id into bj
                                                from backlog in bj.DefaultIfEmpty()
                                                join bp in DbContext.Set<Project>() on backlog.ProjectId equals bp.Id into bpj
                                                from project in bpj.DefaultIfEmpty()

                                                join ip in DbContext.Set<Project>() on i.ProjectId equals ip.Id into ipj
                                                from projects in ipj.DefaultIfEmpty()
                                                orderby un.CreationTime descending
                                                select new UserNotificationViewModel
                                                {
                                                    Id = un.Id,
                                                    Name = un.Name,
                                                    CreatorUsername = u.Name,
                                                    CreatorUserId = u.Id,
                                                    IssueId = i.Id,
                                                    IssueName = i.Name,
                                                    IssueCode = i.Code,
                                                    IssueType = new IssueTypeViewModel
                                                    {
                                                        Id = it.Id,
                                                        Name = it.Name,
                                                        Description = it.Description,
                                                        Icon = it.Icon,
                                                        Level = it.Level,
                                                        IsMain = it.IsMain,
                                                    },
                                                    StatusName = s.Name,
                                                    IsRead = un.IsRead,
                                                    ProjectCode = string.IsNullOrWhiteSpace(pro.Code) ? string.IsNullOrWhiteSpace(project.Code) ? string.IsNullOrWhiteSpace(projects.Code) ? string.Empty : projects.Code : project.Code : pro.Code
                                                }).ToListAsync();

        return userNotificationViewModels.AsReadOnly();
    }

    public async Task<UserNotificationViewModel?> ToUserNotificationViewModeAsync(Guid id)
    {
        var userNotificationViewModel = await (from un in Entity.AsNoTracking().Where(un => un.Id == id)
                                               join u in DbContext.AppUser on un.CreatorUserId equals u.Id
                                               join i in DbContext.Set<Issue>() on un.IssueId equals i.Id
                                               join it in DbContext.Set<IssueType>() on i.IssueTypeId equals it.Id
                                               join s in DbContext.Set<Status>() on i.StatusId equals s.Id

                                               join sp in DbContext.Set<Sprint>() on i.SprintId equals sp.Id into spj
                                               from sprint in spj.DefaultIfEmpty()
                                               join spp in DbContext.Set<Project>() on sprint.ProjectId equals spp.Id into sppj
                                               from pro in sppj.DefaultIfEmpty()

                                               join b in DbContext.Set<Backlog>() on i.BacklogId equals b.Id into bj
                                               from backlog in bj.DefaultIfEmpty()
                                               join bp in DbContext.Set<Project>() on backlog.ProjectId equals bp.Id into bpj
                                               from project in bpj.DefaultIfEmpty()

                                               join ip in DbContext.Set<Project>() on i.ProjectId equals ip.Id into ipj
                                               from projects in ipj.DefaultIfEmpty()
                                               select new UserNotificationViewModel
                                               {
                                                   Id = un.Id,
                                                   Name = u.Name,
                                                   CreatorUsername = u.Name,
                                                   CreatorUserId = u.Id,
                                                   IssueId = i.Id,
                                                   IssueName = i.Name,
                                                   IssueCode = i.Code,
                                                   IssueType = new IssueTypeViewModel
                                                   {
                                                       Id = it.Id,
                                                       Name = it.Name,
                                                       Description = it.Description,
                                                       Icon = it.Icon,
                                                       Level = it.Level,
                                                       IsMain = it.IsMain,
                                                   },
                                                   StatusName = s.Name,
                                                   IsRead = un.IsRead,
                                                   CreationTime = un.CreationTime,
                                                   ProjectCode = string.IsNullOrWhiteSpace(pro.Code) ? string.IsNullOrWhiteSpace(project.Code) ? string.IsNullOrWhiteSpace(projects.Code) ? string.Empty : projects.Code : project.Code : pro.Code
                                               }).FirstOrDefaultAsync();
        return userNotificationViewModel;
    }

    public async Task<int> GetUnreadUserNotificationNumAsync(Guid userId)
    {
        var unreadUserNotificationNum = await Entity
            .AsNoTracking()
            .Where(un => un.UserId == userId && un.IsRead == false)
            .CountAsync();

        return unreadUserNotificationNum;
    }
}
