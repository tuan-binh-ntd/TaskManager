using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface INotificationEventService
{
    Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationEventsByNotificationId(Guid projectId);
    Task<NotificationEventViewModel> Create(Guid notificationId, CreateNotificationEventDto createNotificationEventDto);
    Task<NotificationEventViewModel> Update(Guid id, UpdateNotificationEventDto updateNotificationEventDto);
    Task<Guid> Delete(Guid id);
    Task<NotificationViewModel> GetNotificationViewModelByProjectId(Guid projectId);
    Task<IReadOnlyCollection<IssueEventViewModel>> GetIssueEventViewModels();
}
