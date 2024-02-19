namespace TaskManager.Application.Epics.Events;

internal class EpicUpdatedVersionDomainEventHandler(
    IVersionIssueRepository versionIssueRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedVersionDomainEvent>
{
    private readonly IVersionIssueRepository _versionIssueRepository = versionIssueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedVersionDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UpdateEpicDto.VersionIds is not null && notification.UpdateEpicDto.VersionIds.Count != 0)
        {
            var removedVersionIssues = await _versionIssueRepository.GetVersionIssuesByIssueIdAsync(notification.Epic.Id);
            _versionIssueRepository.RemoveRange(removedVersionIssues);

            var versionIssues = notification.UpdateEpicDto.VersionIds
            .Select(versionId => VersionIssue.Create(versionId, notification.Epic.Id))
            .ToList();

            _versionIssueRepository.InsertRange(versionIssues);
        }
        else if (notification.UpdateEpicDto.VersionIds is not null && notification.UpdateEpicDto.VersionIds.Count == 0)
        {
            var removedVersionIssues = await _versionIssueRepository.GetVersionIssuesByIssueIdAsync(notification.Epic.Id);
            _versionIssueRepository.RemoveRange(removedVersionIssues);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
