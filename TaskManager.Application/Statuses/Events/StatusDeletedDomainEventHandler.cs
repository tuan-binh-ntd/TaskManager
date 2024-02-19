namespace TaskManager.Application.Statuses.Events;

internal sealed class StatusDeletedDomainEventHandler(
    IIssueRepository issueRepository
    )
    : IDomainEventHandler<StatusDeletedDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;

    public async Task Handle(StatusDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _issueRepository.UpdateOneColumnForIssueAsync(notification.StatusId, notification.NewStatusId, NameColumn.StatusId);
    }
}
