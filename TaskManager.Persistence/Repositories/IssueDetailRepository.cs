namespace TaskManager.Persistence.Repositories;

public class IssueDetailRepository(IDbContext context) : GenericRepository<IssueDetail>(context)
    , IIssueDetailRepository
{
    public async Task<CurrentAssigneeAndReporterViewModel?> GetCurrentAssigneeAndReporterAsync(Guid issueId)
    {
        var currentAssigneeAndReporterViewModel = await Entity
            .Where(id => id.IssueId == issueId)
            .Select(id => new CurrentAssigneeAndReporterViewModel
            {
                CurrentAssigness = id.AssigneeId,
                Reporter = id.ReporterId,
            })
            .FirstOrDefaultAsync();

        return currentAssigneeAndReporterViewModel;
    }

    public async Task<Guid?> GetCurrentAssigneeIdAsync(Guid issueId)
    {
        var currentAssigneeId = await Entity
            .Where(id => id.IssueId == issueId)
            .Select(id => id.AssigneeId)
            .FirstOrDefaultAsync();
        return currentAssigneeId;
    }

    public async Task<IssueDetail?> GetIssueDetailByIssueIdAsync(Guid issueId)
    {
        return await Entity
            .Where(id => id.IssueId == issueId)
            .FirstOrDefaultAsync();
    }

    public async Task<Guid> GetReporterIdAsync(Guid issueId)
    {
        var reporterId = await Entity
            .Where(id => id.IssueId == issueId)
            .Select(id => id.ReporterId)
            .FirstOrDefaultAsync();
        return reporterId;
    }

    public async Task UpdateOneColumnForIssueDetailAsync(Guid oldValue, Guid? newValue, NameColumn nameColumn)
    {
        switch (nameColumn)
        {
            case NameColumn.AssigneeId:
                await Entity
                    .Where(i => i.AssigneeId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.AssigneeId, newValue));
                break;
            case NameColumn.ReporterId:
                await Entity
                    .Where(i => i.ReporterId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.ReporterId, newValue));
                break;
        }
    }
}
