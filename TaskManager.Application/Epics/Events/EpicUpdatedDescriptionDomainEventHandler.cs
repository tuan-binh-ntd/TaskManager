namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedDescriptionDomainEventHandler(
    IIssueHistoryRepository issueHistoryRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedDescriptionDomainEvent>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedDescriptionDomainEvent notification, CancellationToken cancellationToken)
    {
        var updatedTheDescriptionHis = IssueHistory.Create(IssueConstants.Description_IssueHistoryName,
            notification.UpdateEpicDto.ModificationUserId,
            $@"{notification.Epic.Description
            ?? IssueConstants.None_IssueHistoryContent} to 
            {notification.UpdateEpicDto.Description
            ?? IssueConstants.None_IssueHistoryContent}",
            notification.Epic.Id
        );

        _issueHistoryRepository.Insert(updatedTheDescriptionHis);

        notification.Epic.Description = notification.Epic.Description;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
