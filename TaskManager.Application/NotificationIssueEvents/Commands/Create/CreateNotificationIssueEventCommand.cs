namespace TaskManager.Application.NotificationIssueEvents.Commands.Create;

public sealed class CreateNotificationIssueEventCommand(
    Guid notificationId,
    CreateNotificationEventDto createNotificationEventDto
    )
    : ICommand<Result<NotificationEventViewModel>>
{
    public Guid NotificationId { get; private set; } = notificationId;
    public CreateNotificationEventDto CreateNotificationEventDto { get; private set; } = createNotificationEventDto;
}
