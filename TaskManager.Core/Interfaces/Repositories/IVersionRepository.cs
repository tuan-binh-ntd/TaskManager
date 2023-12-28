using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;
using Version = TaskManager.Core.Entities.Version;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IVersionRepository : IRepository<Version>
{
    Task<IReadOnlyCollection<Version>> GetByProjectId(Guid projectId);
    Task<Version> GetById(Guid id);
    Version Add(Version version);
    void Update(Version version);
    void Delete(Guid id);
    void AddRange(IReadOnlyCollection<VersionIssue> versionIssues);
    Task<IReadOnlyCollection<Guid>> GetIssueIdsByVersionId(Guid versionId);
    void AddVersionIssue(VersionIssue versionIssue);
    void RemoveRange(IReadOnlyCollection<VersionIssue> versionIssues);
    Task<IReadOnlyCollection<VersionIssue>> GetVersionIssuesByIssueId(Guid issueId);
    Task<IReadOnlyCollection<VersionViewModel>> GetStatusViewModelsByIssueId(Guid issueId);
    Task<IReadOnlyCollection<VersionIssue>> GetVersionIssuesByVersionId(Guid versionId);
    Task<int> IssuesNotDoneNumInVersion(Guid versionId);
    Task<IReadOnlyCollection<Guid>> IssuesNotDoneInVersion(Guid versionId);
    Task UpdateVersionIssuesByIssueIds(IReadOnlyCollection<Guid> issueIds, Guid newVersionId);
}
