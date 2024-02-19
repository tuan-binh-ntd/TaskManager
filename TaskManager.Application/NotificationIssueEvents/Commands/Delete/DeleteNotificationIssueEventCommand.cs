namespace TaskManager.Application.NotificationIssueEvents.Commands.Delete;

public sealed class DeleteNotificationIssueEventCommand(
    Guid notificationIssueEventId
    )
    : ICommand<Result<Guid>>
{
    public Guid NotificationIssueEventId { get; private set; } = notificationIssueEventId;
}
