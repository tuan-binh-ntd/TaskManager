namespace TaskManager.Application.NotificationIssueEvents.Commands.Delete;

internal sealed class DeleteNotificationIssueEventCommandHandler(
    INotificationIssueEventRepository notificationIssueEventRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteNotificationIssueEventCommand, Result<Guid>>
{
    private readonly INotificationIssueEventRepository _notificationIssueEventRepository = notificationIssueEventRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteNotificationIssueEventCommand deleteNotificationIssueEventCommand, CancellationToken cancellationToken)
    {
        var notificationIssueEvent = await _notificationIssueEventRepository.GetByIdAsync(deleteNotificationIssueEventCommand.NotificationIssueEventId);
        if (notificationIssueEvent is null) return Result.Failure<Guid>(Error.NotFound);
        _notificationIssueEventRepository.Remove(notificationIssueEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(notificationIssueEvent.Id);
    }
}
