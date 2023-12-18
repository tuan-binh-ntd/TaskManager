using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<Notification> GetById(Guid id);
    Notification Add(Notification notification);
    void Update(Notification notification);
    void Delete(Guid id);
    Task<IReadOnlyCollection<NotificationEventViewModel>> GetByNotificationId(Guid id);
    void Add(NotificationIssueEvent notificationIssueEvent);
    void Update(NotificationIssueEvent notificationIssueEvent);
    void DeleteNotificationIssueEvent(NotificationIssueEvent notificationIssueEvent);
    Task<NotificationIssueEvent?> GetNotificationIssueEventById(Guid id);
    Task<string?> GetIssueEventName(Guid issueEventId);
    Task<NotificationViewModel> GetByProjectId(Guid projectId);
    Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationIssueEventByProjectId(Guid projectId);
}
