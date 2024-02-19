namespace TaskManager.Application.Issues.Events.IssueCreatedDomainEvents;

internal class CreateIssueHistoryDomainHandler(
    IIssueHistoryRepository issueHistoryRepository,
    ISprintRepository sprintRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueCreatedDomainEvent>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var issueHis = IssueHistory.Create(IssueConstants.Created_IssueHistoryName,
            notification.CreatorUserId,
            string.Empty,
            notification.Issue.Id
            );
        _issueHistoryRepository.Insert(issueHis);

        if (notification.Issue.SprintId is Guid newSprintId)
        {
            string? newSprintName = await _sprintRepository.GetNameOfSprintAsync(newSprintId);
            var changedTheParentHis = IssueHistory.Create(IssueConstants.Parent_IssueHistoryName,
                notification.CreatorUserId,
                $"{IssueConstants.None_IssueHistoryContent} to {newSprintName}",
                notification.Issue.Id);
            _issueHistoryRepository.Insert(changedTheParentHis);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
