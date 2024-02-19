namespace TaskManager.Application.NotificationIssueEvents.Commands.Update;

public sealed class UpdateNotificationIssueEventCommand(
    Guid notificationIssueEventId,
    UpdateNotificationEventDto updateNotificationEventDto
    )
    : ICommand<Result<NotificationEventViewModel>>
{
    public Guid NotificationIssueEventId { get; private set; } = notificationIssueEventId;
    public UpdateNotificationEventDto UpdateNotificationEventDto { get; private set; } = updateNotificationEventDto;
}
