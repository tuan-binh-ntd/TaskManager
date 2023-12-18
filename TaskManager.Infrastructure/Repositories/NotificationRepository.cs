using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Notification Add(Notification notification)
    {
        return _context.Notifications.Add(notification).Entity;
    }

    public void Delete(Guid id)
    {
        var notification = _context.Notifications.FirstOrDefault(o => o.Id == id);
        _context.Notifications.Remove(notification!);
    }

    public async Task<Notification> GetById(Guid id)
    {
        var notification = await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
        return notification!;
    }

    public void Update(Notification notification)
    {
        _context.Entry(notification).State = EntityState.Modified;
    }

    public async Task<IReadOnlyCollection<NotificationEventViewModel>> GetByNotificationId(Guid id)
    {
        var notificationEventViewModels = await (from n in _context.Notifications.AsNoTracking().Where(n => n.Id == id)
                                                 join nie in _context.NotificationIssueEvents.AsNoTracking() on n.Id equals nie.NotificationId
                                                 join ie in _context.IssueEvents.AsNoTracking() on nie.IssueEventId equals ie.Id
                                                 select new NotificationEventViewModel
                                                 {
                                                     Id = nie.Id,
                                                     EventName = ie.Name,
                                                     EventId = ie.Id,
                                                     AllWatcher = nie.AllWatcher,
                                                     CurrentAssignee = nie.CurrentAssignee,
                                                     Reporter = nie.Reporter,
                                                     ProjectLead = nie.ProjectLead,
                                                 }).ToListAsync();

        return notificationEventViewModels.AsReadOnly();
    }

    public void Add(NotificationIssueEvent notificationIssueEvent)
    {
        _context.NotificationIssueEvents.Add(notificationIssueEvent);
    }

    public void Update(NotificationIssueEvent notificationIssueEvent)
    {
        _context.Entry(notificationIssueEvent).State = EntityState.Modified;
    }

    public void DeleteNotificationIssueEvent(NotificationIssueEvent notificationIssueEvent)
    {
        _context.NotificationIssueEvents.Remove(notificationIssueEvent);
    }

    public async Task<NotificationIssueEvent?> GetNotificationIssueEventById(Guid id)
    {
        var notificationIssueEvent = await _context.NotificationIssueEvents.Where(nie => nie.Id == id).FirstOrDefaultAsync();
        return notificationIssueEvent;
    }

    public async Task<string?> GetIssueEventName(Guid issueEventId)
    {
        string? name = await _context.IssueEvents.Where(ie => ie.Id == issueEventId).Select(ie => ie.Name).FirstOrDefaultAsync();
        return name;
    }

    public async Task<NotificationViewModel> GetByProjectId(Guid projectId)
    {
        var notification = await (from n in _context.Notifications.AsNoTracking().Where(n => n.ProjectId == projectId)
                                  join nie in _context.NotificationIssueEvents.AsNoTracking() on n.Id equals nie.NotificationId
                                  join ie in _context.IssueEvents.AsNoTracking() on nie.IssueEventId equals ie.Id
                                  select new NotificationViewModel
                                  {
                                      Id = n.Id,
                                      Name = n.Name,
                                      Description = n.Description,
                                      NotificationEvent = _context.NotificationIssueEvents.Select(sd => new NotificationEventViewModel
                                      {
                                          Id = nie.Id,
                                          EventId = ie.Id,
                                          EventName = ie.Name,
                                          AllWatcher = nie.AllWatcher,
                                          CurrentAssignee = nie.CurrentAssignee,
                                          Reporter = nie.Reporter,
                                          ProjectLead = nie.ProjectLead,
                                      }).ToList()
                                  }).FirstOrDefaultAsync();

        return notification!;
    }

    public async Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationIssueEventByProjectId(Guid projectId)
    {
        var notificationEventViewModels = await (from n in _context.Notifications.AsNoTracking().Where(n => n.ProjectId == projectId)
                                                 join nie in _context.NotificationIssueEvents.AsNoTracking() on n.Id equals nie.NotificationId
                                                 join ie in _context.IssueEvents.AsNoTracking() on nie.IssueEventId equals ie.Id
                                                 select new NotificationEventViewModel
                                                 {
                                                     Id = nie.Id,
                                                     EventName = ie.Name,
                                                     EventId = ie.Id,
                                                     AllWatcher = nie.AllWatcher,
                                                     CurrentAssignee = nie.CurrentAssignee,
                                                     Reporter = nie.Reporter,
                                                     ProjectLead = nie.ProjectLead,
                                                 }).ToListAsync();

        return notificationEventViewModels.AsReadOnly();
    }
}
