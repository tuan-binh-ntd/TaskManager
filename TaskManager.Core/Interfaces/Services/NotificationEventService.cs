using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public class NotificationEventService : INotificationEventService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationEventService(
        INotificationRepository notificationRepository
        )
    {
        _notificationRepository = notificationRepository;
    }

    #region Private methods
    private async Task<NotificationEventViewModel> ToviewModel(NotificationIssueEvent notificationIssueEvent)
    {
        return new NotificationEventViewModel
        {
            Id = notificationIssueEvent.Id,
            AllWatcher = notificationIssueEvent.AllWatcher,
            CurrentAssignee = notificationIssueEvent.CurrentAssignee,
            Reporter = notificationIssueEvent.Reporter,
            ProjectLead = notificationIssueEvent.ProjectLead,
            EventName = await _notificationRepository.GetIssueEventName(notificationIssueEvent.IssueEventId) ?? string.Empty
        };
    }
    #endregion
    public async Task<NotificationEventViewModel> Create(Guid notificationId, CreateNotificationEventDto createNotificationEventDto)
    {
        var notificationIssueEvent = new NotificationIssueEvent
        {
            NotificationId = notificationId,
            IssueEventId = createNotificationEventDto.EventId,
            AllWatcher = createNotificationEventDto.AllWatcher,
            CurrentAssignee = createNotificationEventDto.CurrentAssignee,
            Reporter = createNotificationEventDto.Reporter,
            ProjectLead = createNotificationEventDto.ProjectLead,
        };

        _notificationRepository.Add(notificationIssueEvent);
        await _notificationRepository.UnitOfWork.SaveChangesAsync();
        return await ToviewModel(notificationIssueEvent);
    }

    public async Task<Guid> Delete(Guid id)
    {
        var notificationIssueEvent = await _notificationRepository.GetNotificationIssueEventById(id) ?? throw new NotificationIssueEventNullException();
        _notificationRepository.DeleteNotificationIssueEvent(notificationIssueEvent);
        await _notificationRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<IReadOnlyCollection<NotificationEventViewModel>> GetNotificationEventsByNotificationId(Guid id)
    {
        var notificationEventViewModels = await _notificationRepository.GetByNotificationId(id);
        return notificationEventViewModels;
    }

    public async Task<NotificationEventViewModel> Update(Guid id, UpdateNotificationEventDto updateNotificationEventDto)
    {
        var notificationIssueEvent = await _notificationRepository.GetNotificationIssueEventById(id) ?? throw new NotificationIssueEventNullException();

        notificationIssueEvent.AllWatcher = updateNotificationEventDto.AllWatcher;
        notificationIssueEvent.CurrentAssignee = updateNotificationEventDto.CurrentAssignee;
        notificationIssueEvent.Reporter = updateNotificationEventDto.Reporter;
        notificationIssueEvent.ProjectLead = updateNotificationEventDto.ProjectLead;

        _notificationRepository.Update(notificationIssueEvent);
        await _notificationRepository.UnitOfWork.SaveChangesAsync();

        return await ToviewModel(notificationIssueEvent);
    }

    public async Task<NotificationViewModel> GetNotificationViewModelByProjectId(Guid projectId)
    {
        var notification = await _notificationRepository.GetByProjectId(projectId);
        return notification;
    }
}
