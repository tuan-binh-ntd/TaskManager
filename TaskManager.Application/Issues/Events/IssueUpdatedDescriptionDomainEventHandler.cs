namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedDescriptionDomainEventHandler(
    IIssueHistoryRepository issueHistoryRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedDescriptionDomainEvent>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedDescriptionDomainEvent notification, CancellationToken cancellationToken)
    {
        var updatedTheDescriptionHis = IssueHistory.Create(IssueConstants.Description_IssueHistoryName,
            notification.UpdateIssueDto.ModificationUserId,
            string.IsNullOrWhiteSpace(notification.Issue.Description) ? $"{IssueConstants.None_IssueHistoryContent} to {notification.UpdateIssueDto.Description}" : $"{notification.Issue.Description} to {notification.UpdateIssueDto.Description}",
            notification.Issue.Id);
        _issueHistoryRepository.Insert(updatedTheDescriptionHis);

        notification.Issue.Description = notification.UpdateIssueDto.Description;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
