namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedVersionDomainEventHandler(
    IVersionIssueRepository versionIssueRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedVersionDomainEvent>
{
    private readonly IVersionIssueRepository _versionIssueRepository = versionIssueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedVersionDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UpdateIssueDto.VersionIds is not null && notification.UpdateIssueDto.VersionIds.Count != 0)
        {
            var removedVersionIssues = await _versionIssueRepository.GetVersionIssuesByIssueIdAsync(notification.Issue.Id);
            _versionIssueRepository.RemoveRange(removedVersionIssues);

            var versionIssues = notification.UpdateIssueDto.VersionIds
            .Select(versionId => VersionIssue.Create(versionId, notification.Issue.Id))
            .ToList();

            _versionIssueRepository.InsertRange(versionIssues);
        }
        else if (notification.UpdateIssueDto.VersionIds is not null && notification.UpdateIssueDto.VersionIds.Count == 0)
        {
            var removedVersionIssues = await _versionIssueRepository.GetVersionIssuesByIssueIdAsync(notification.Issue.Id);
            _versionIssueRepository.RemoveRange(removedVersionIssues);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
