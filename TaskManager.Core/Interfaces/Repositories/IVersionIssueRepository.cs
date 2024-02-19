namespace TaskManager.Core.Interfaces.Repositories;

public interface IVersionIssueRepository
{
    Task<IReadOnlyCollection<VersionIssue>> GetVersionIssuesByIssueIdAsync(Guid issueId);
    Task<IReadOnlyCollection<VersionIssue>> GetVersionIssuesByVersionIdAsync(Guid versionId);
    Task<IReadOnlyCollection<Guid>> GetIssueIdsByVersionIdAsync(Guid versionId);
    Task UpdateVersionIssuesByIssueIdsAsync(IReadOnlyCollection<Guid> issueIds, Guid newVersionId);
    void RemoveRange(IReadOnlyCollection<VersionIssue> versionIssues);
    void InsertRange(IReadOnlyCollection<VersionIssue> versionIssues);
    Task UpdateVersionIdAsync(Guid oldVersionId, Guid newVersionId);
}
