namespace TaskManager.Application.NotificationIssueEvents.Commands.Update;

internal sealed class UpdateNotificationIssueEventCommandHandler(
    INotificationIssueEventRepository notificationIssueEventRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateNotificationIssueEventCommand, Result<NotificationEventViewModel>>
{
    private readonly INotificationIssueEventRepository _notificationIssueEventRepository = notificationIssueEventRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<NotificationEventViewModel>> Handle(UpdateNotificationIssueEventCommand updateNotificationIssueEventCommand, CancellationToken cancellationToken)
    {
        var notificationIssueEvent = await _notificationIssueEventRepository.GetByIdAsync(updateNotificationIssueEventCommand.NotificationIssueEventId);
        if (notificationIssueEvent is null) return Result.Failure<NotificationEventViewModel>(Error.NotFound);
        notificationIssueEvent.AllWatcher = updateNotificationIssueEventCommand.UpdateNotificationEventDto.AllWatcher;
        notificationIssueEvent.CurrentAssignee = updateNotificationIssueEventCommand.UpdateNotificationEventDto.CurrentAssignee;
        notificationIssueEvent.Reporter = updateNotificationIssueEventCommand.UpdateNotificationEventDto.Reporter;
        notificationIssueEvent.ProjectLead = updateNotificationIssueEventCommand.UpdateNotificationEventDto.ProjectLead;

        _notificationIssueEventRepository.Update(notificationIssueEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(notificationIssueEvent.Adapt<NotificationEventViewModel>());
    }
}
