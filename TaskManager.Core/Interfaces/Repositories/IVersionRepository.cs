namespace TaskManager.Core.Interfaces.Repositories;

public interface IVersionRepository
{
    Task<IReadOnlyCollection<Version>> GetVersionsByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<VersionViewModel>> GetStatusViewModelsByIssueIdAsync(Guid issueId);
    Task<int> IssuesNotDoneNumInVersionAsync(Guid versionId);
    Task<IReadOnlyCollection<Guid>> IssuesNotDoneInVersionAsync(Guid versionId);
    Task<Version?> GetByIdAsync(Guid id);
    void Insert(Version version);
    void Update(Version version);
    void Remove(Version version);
}
