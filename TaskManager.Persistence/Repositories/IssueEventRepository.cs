namespace TaskManager.Persistence.Repositories;

public class IssueEventRepository(IDbContext context) : GenericRepository<IssueEvent>(context)
    , IIssueEventRepository
{

    public async Task<IReadOnlyCollection<IssueEvent>> GetIssueEventsAsync()
    {
        var issueEvents = await Entity
            .AsNoTracking()
            .ToListAsync();

        return issueEvents.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<IssueEventViewModel>> GetIssueEventViewModelsAsync()
    {
        var issueEventViewModels = await Entity
            .AsNoTracking()
            .Select(ie => new IssueEventViewModel
            {
                Id = ie.Id,
                Name = ie.Name,
            }).ToListAsync();

        return issueEventViewModels.AsReadOnly();
    }
}
