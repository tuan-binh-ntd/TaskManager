namespace TaskManager.Core.Interfaces.Repositories;

public interface INotificationIssueEventRepository
{
    Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationIssueEventsByProjectIdAsync(Guid projectId);
    Task<NotificationEventViewModel?> GetSomeoneAddedAnAttachmentEventByProjectIdAsync(Guid projectId);
    Task<NotificationIssueEvent?> GetByIdAsync(Guid id);
    void Insert(NotificationIssueEvent notificationIssueEvent);
    void Update(NotificationIssueEvent notificationIssueEvent);
    void Remove(NotificationIssueEvent notificationIssueEvent);
}
