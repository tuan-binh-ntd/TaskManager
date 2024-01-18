namespace TaskManager.Infrastructure.Repositories;

public class IssueEventRepository : IIssueEventRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public IssueEventRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyCollection<IssueEvent>> Gets()
    {
        var issueEvents = await _context.IssueEvents.AsNoTracking().ToListAsync();
        return issueEvents.AsReadOnly();
    }
}
