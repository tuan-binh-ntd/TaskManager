namespace TaskManager.Persistence.Repositories;

public class LabelIssueRepository(IDbContext context) : GenericRepository<LabelIssue>(context)
    , ILabelIssueRepository
{
    public async Task<IReadOnlyCollection<LabelIssue>> GetLabelIssuesByIssueIdAsync(Guid issueId)
    {
        var labelIssues = await Entity
            .Where(li => li.IssueId == issueId)
            .ToListAsync();

        return labelIssues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<LabelIssue>> GetLabelIssuesByLabelIdAsync(Guid labelId)
    {
        var labelIssues = await Entity
            .Where(li => li.LabelId == labelId)
            .ToListAsync();

        return labelIssues.AsReadOnly();
    }
}
