namespace TaskManager.Persistence.Repositories
{
    internal class NotificationIssueEventRepository(IDbContext context) : GenericRepository<NotificationIssueEvent>(context)
        , INotificationIssueEventRepository
    {
        public async Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationIssueEventsByProjectIdAsync(Guid projectId)
        {
            var notificationEventViewModels = await (from n in DbContext.Set<Notification>().AsNoTracking().Where(n => n.ProjectId == projectId)
                                                     join nie in Entity.AsNoTracking() on n.Id equals nie.NotificationId
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

        public async Task<NotificationEventViewModel?> GetSomeoneAddedAnAttachmentEventByProjectIdAsync(Guid projectId)
        {
            var notificationEventViewModel = await (from n in DbContext.Set<Notification>().AsNoTracking().Where(n => n.ProjectId == projectId)
                                                    join nie in Entity.AsNoTracking() on n.Id equals nie.NotificationId
                                                    join ie in DbContext.Set<IssueEvent>().AsNoTracking().Where(ie => ie.Name == IssueEventConstants.SomeoneMadeAnAttachmentName) on nie.IssueEventId equals ie.Id
                                                    select new NotificationEventViewModel
                                                    {
                                                        AllWatcher = nie.AllWatcher,
                                                        CurrentAssignee = nie.CurrentAssignee,
                                                        Reporter = nie.Reporter,
                                                        ProjectLead = nie.ProjectLead,
                                                    })
                                                     .FirstOrDefaultAsync();

            return notificationEventViewModel;
        }
    }
}
