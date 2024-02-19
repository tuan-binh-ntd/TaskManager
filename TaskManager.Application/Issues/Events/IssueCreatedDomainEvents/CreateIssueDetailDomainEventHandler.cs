namespace TaskManager.Application.Issues.Events.IssueCreatedDomainEvents;

internal sealed class CreateIssueDetailDomainEventHandler(
    IIssueDetailRepository issueDetailRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueCreatedDomainEvent>
{
    private readonly IIssueDetailRepository _issueDetailRepository = issueDetailRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var issueDetail = IssueDetail.Create(notification.DefaultAssigneeId,
            notification.CreatorUserId,
            0,
            notification.Issue.Id);

        _issueDetailRepository.Insert(issueDetail);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
