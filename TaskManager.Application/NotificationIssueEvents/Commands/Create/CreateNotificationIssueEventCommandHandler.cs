namespace TaskManager.Application.NotificationIssueEvents.Commands.Create;

internal sealed class CreateNotificationIssueEventCommandHandler(
    INotificationIssueEventRepository notificationIssueEventRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateNotificationIssueEventCommand, Result<NotificationEventViewModel>>
{
    private readonly INotificationIssueEventRepository _notificationIssueEventRepository = notificationIssueEventRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<NotificationEventViewModel>> Handle(CreateNotificationIssueEventCommand createNotificationIssueEventCommand, CancellationToken cancellationToken)
    {
        var notificationIssueEvent = NotificationIssueEvent.Create(createNotificationIssueEventCommand.CreateNotificationEventDto.AllWatcher,
            createNotificationIssueEventCommand.CreateNotificationEventDto.CurrentAssignee,
            createNotificationIssueEventCommand.CreateNotificationEventDto.Reporter,
            createNotificationIssueEventCommand.CreateNotificationEventDto.ProjectLead,
            createNotificationIssueEventCommand.NotificationId,
            createNotificationIssueEventCommand.CreateNotificationEventDto.EventId
            );

        _notificationIssueEventRepository.Insert(notificationIssueEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(notificationIssueEvent.Adapt<NotificationEventViewModel>());
    }
}
