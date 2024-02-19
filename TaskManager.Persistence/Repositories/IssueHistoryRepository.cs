namespace TaskManager.Persistence.Repositories;

public class IssueHistoryRepository(IDbContext context) : GenericRepository<IssueHistory>(context)
    , IIssueHistoryRepository
{

    public async Task<IReadOnlyCollection<IssueHistoryViewModel>> Gets()
    {
        var issueHistories = await Entity
            .AsNoTracking()
            .Select(e => new IssueHistoryViewModel
            {
                Id = e.Id,
                CreatorUserId = e.CreatorUserId,
                CreationTime = e.CreationTime,
                Content = e.Content
            })
            .ToListAsync();

        return issueHistories.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<IssueHistory>> GetIssueHistoriesByIssueIdAsync(Guid issueId)
    {
        var issueHistories = await Entity
            .AsNoTracking()
            .Where(ih => ih.IssueId == issueId)
            .OrderBy(ih => ih.CreationTime)
            .ToListAsync();

        return issueHistories.AsReadOnly();
    }
}
