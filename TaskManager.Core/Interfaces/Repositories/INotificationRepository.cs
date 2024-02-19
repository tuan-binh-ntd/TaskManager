namespace TaskManager.Core.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationEventViewModelsByNotificationIdAsync(Guid id);
    Task<NotificationEventViewModel?> GetNotificationConfigurationByProjectIdAndEventNameAsync(Guid projectId, string eventName);
    Task<Notification?> GetByIdAsync(Guid id);
    void Insert(Notification notification);
    void Update(Notification notification);
    void Remove(Notification notification);
}
