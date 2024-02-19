namespace TaskManager.Persistence.Repositories;

public class NotificationRepository(IDbContext context) : GenericRepository<Notification>(context)
    , INotificationRepository
{
    public async Task<NotificationEventViewModel?> GetNotificationConfigurationByProjectIdAndEventNameAsync(Guid projectId, string eventName)
    {
        var notificationEventViewModel = await (from n in Entity.AsNoTracking().Where(n => n.ProjectId == projectId)
                                                join nie in DbContext.Set<NotificationIssueEvent>().AsNoTracking() on n.Id equals nie.NotificationId
                                                join ie in DbContext.Set<IssueEvent>().AsNoTracking().Where(ie => ie.Name == eventName) on nie.IssueEventId equals ie.Id
                                                select new NotificationEventViewModel
                                                {
                                                    AllWatcher = nie.AllWatcher,
                                                    CurrentAssignee = nie.CurrentAssignee,
                                                    Reporter = nie.Reporter,
                                                    ProjectLead = nie.ProjectLead,
                                                }).FirstOrDefaultAsync();

        return notificationEventViewModel;
    }

    public async Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationEventViewModelsByNotificationIdAsync(Guid id)
    {
        var notificationEventViewModels = await (from n in Entity.AsNoTracking().Where(n => n.Id == id)
                                                 join nie in DbContext.Set<NotificationIssueEvent>().AsNoTracking() on n.Id equals nie.NotificationId
                                                 join ie in DbContext.Set<IssueEvent>().AsNoTracking() on nie.IssueEventId equals ie.Id
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
