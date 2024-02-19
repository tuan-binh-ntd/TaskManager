namespace TaskManager.Persistence.Repositories;

internal class VersionIssueRepository(IDbContext context) : GenericRepository<VersionIssue>(context)
    , IVersionIssueRepository
{
    public async Task<IReadOnlyCollection<Guid>> GetIssueIdsByVersionIdAsync(Guid versionId)
    {
        var issueIds = await Entity
            .Where(vi => vi.VersionId == versionId)
            .Select(vi => vi.IssueId)
            .ToListAsync();

        return issueIds.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<VersionIssue>> GetVersionIssuesByIssueIdAsync(Guid issueId)
    {
        var versionIssues = await Entity
            .Where(vi => vi.IssueId == issueId)
            .ToListAsync();

        return versionIssues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<VersionIssue>> GetVersionIssuesByVersionIdAsync(Guid versionId)
    {
        var versionIssues = await Entity
            .Where(vi => vi.VersionId == versionId)
            .ToListAsync();

        return versionIssues.AsReadOnly();
    }

    public async Task UpdateVersionIdAsync(Guid oldVersionId, Guid newVersionId)
    {
        await Entity
            .Where(vi => vi.VersionId == oldVersionId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.VersionId, newVersionId));
    }

    public async Task UpdateVersionIssuesByIssueIdsAsync(IReadOnlyCollection<Guid> issueIds, Guid newVersionId)
    {
        await Entity
            .Where(vi => issueIds.Contains(vi.IssueId))
            .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.VersionId, newVersionId));
    }
}
