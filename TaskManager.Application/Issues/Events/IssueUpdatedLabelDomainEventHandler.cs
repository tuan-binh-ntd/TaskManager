namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedLabelDomainEventHandler(
    ILabelIssueRepository labelIssueRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedLabelDomainEvent>
{
    private readonly ILabelIssueRepository _labelIssueRepository = labelIssueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedLabelDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UpdateIssueDto.LabelIds is not null && notification.UpdateIssueDto.LabelIds.Count != 0)
        {
            var removedLabelIssues = await _labelIssueRepository.GetLabelIssuesByIssueIdAsync(notification.Issue.Id);
            _labelIssueRepository.RemoveRange(removedLabelIssues);

            var labelIssues = notification.UpdateIssueDto.LabelIds
                .Select(labelId => LabelIssue.Create(labelId, notification.Issue.Id))
            .ToList();

            _labelIssueRepository.InsertRange(labelIssues);
        }
        else if (notification.UpdateIssueDto.LabelIds is not null && notification.UpdateIssueDto.LabelIds.Count == 0)
        {
            var removedLabelIssues = await _labelIssueRepository.GetLabelIssuesByIssueIdAsync(notification.Issue.Id);

            _labelIssueRepository.RemoveRange(removedLabelIssues);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
