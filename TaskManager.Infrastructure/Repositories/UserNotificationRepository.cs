using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class UserNotificationRepository : IUserNotificationRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public UserNotificationRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(UserNotification userNotification)
    {
        _context.Add(userNotification);
    }

    public void Delete(UserNotification userNotification)
    {
        _context.Remove(userNotification);
    }

    public async Task<UserNotification?> GetById(Guid id)
    {
        var userNotification = await _context.UserNotifications.Where(un => un.Id == id).FirstOrDefaultAsync();
        return userNotification;
    }

    public async Task<IReadOnlyCollection<UserNotificationViewModel>> GetByUserId(Guid userId)
    {
        var userNotificationViewModels = await (from un in _context.UserNotifications.AsNoTracking().Where(un => un.UserId == userId)
                                                join u in _context.Users on un.CreatorUserId equals u.Id
                                                join i in _context.Issues on un.IssueId equals i.Id
                                                join it in _context.IssueTypes on i.IssueTypeId equals it.Id
                                                join s in _context.Statuses on i.StatusId equals s.Id
                                                select new UserNotificationViewModel
                                                {
                                                    Id = un.Id,
                                                    Name = u.Name,
                                                    CreatorUsername = u.Name,
                                                    CreatorUserId = u.Id,
                                                    IssueId = u.Id,
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
                                                }).ToListAsync();

        return userNotificationViewModels.AsReadOnly();
    }

    public void Update(UserNotification userNotification)
    {
        _context.Entry(userNotification).State = EntityState.Modified;
    }

    public void AddRange(IReadOnlyCollection<UserNotification> userNotifications)
    {
        _context.UserNotifications.AddRange(userNotifications);
    }

    public async Task<UserNotificationViewModel?> ToUserNotificationViewMode(Guid id)
    {
        var userNotificationViewModel = await (from un in _context.UserNotifications.AsNoTracking().Where(un => un.Id == id)
                                               join u in _context.Users on un.CreatorUserId equals u.Id
                                               join i in _context.Issues on un.IssueId equals i.Id
                                               join it in _context.IssueTypes on i.IssueTypeId equals it.Id
                                               join s in _context.Statuses on i.StatusId equals s.Id
                                               select new UserNotificationViewModel
                                               {
                                                   Id = un.Id,
                                                   Name = u.Name,
                                                   CreatorUsername = u.Name,
                                                   CreatorUserId = u.Id,
                                                   IssueId = u.Id,
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
                                               }).FirstOrDefaultAsync();
        return userNotificationViewModel;
    }

    public async Task<int> GetUnreadUserNotificationNum(Guid userId)
    {
        var unreadUserNotificationNum = await _context.UserNotifications.AsNoTracking().Where(un => un.UserId == userId && un.IsRead == false).CountAsync();

        return unreadUserNotificationNum;
    }
}
