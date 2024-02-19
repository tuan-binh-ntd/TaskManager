namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedLabelDomainEventHandler(
    ILabelIssueRepository labelIssueRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedLabelDomainEvent>
{
    private readonly ILabelIssueRepository _labelIssueRepository = labelIssueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedLabelDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UpdateEpicDto.LabelIds is not null && notification.UpdateEpicDto.LabelIds.Count != 0)
        {
            var removedLabelIssues = await _labelIssueRepository.GetLabelIssuesByIssueIdAsync(notification.Epic.Id);
            _labelIssueRepository.RemoveRange(removedLabelIssues);

            var labelIssues = notification.UpdateEpicDto.LabelIds
                .Select(labelId => LabelIssue.Create(labelId, notification.Epic.Id))
            .ToList();

            _labelIssueRepository.InsertRange(labelIssues);
        }
        else if (notification.UpdateEpicDto.LabelIds is not null && notification.UpdateEpicDto.LabelIds.Count == 0)
        {
            var removedLabelIssues = await _labelIssueRepository.GetLabelIssuesByIssueIdAsync(notification.Epic.Id);

            _labelIssueRepository.RemoveRange(removedLabelIssues);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
