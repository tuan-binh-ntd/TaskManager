using TaskManager.Core;

namespace TaskManager.Infrastructure.Repositories;

public class IssueDetailRepository : IIssueDetailRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public IssueDetailRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IssueDetailViewModel Add(IssueDetail issueDetail)
    {
        return _context.IssueDetails.Add(issueDetail).Entity.Adapt<IssueDetailViewModel>();
    }

    public void Delete(Guid id)
    {
        var issueDetail = _context.IssueDetails.SingleOrDefault(e => e.Id == id);
        _context.IssueDetails.Remove(issueDetail!);
    }

    public async Task<IReadOnlyCollection<IssueDetailViewModel>> Gets()
    {
        var issueDetails = await _context.IssueDetails.AsNoTracking().ProjectToType<IssueDetailViewModel>().ToListAsync();
        return issueDetails.AsReadOnly();
    }

    public void Update(IssueDetail issueDetail)
    {
        _context.Entry(issueDetail).State = EntityState.Modified;
    }

    public async Task<IssueDetail> GetById(Guid id)
    {
        var issueDetail = await _context.IssueDetails.Where(i => i.IssueId == id).FirstOrDefaultAsync();
        return issueDetail!;
    }

    public async Task<CurrentAssigneeAndReporterViewModel?> GetCurrentAssigneeAndReporter(Guid issueId)
    {
        var currentAssigneeAndReporterViewModel = _context.IssueDetails
            .Where(id => id.IssueId == issueId)
            .Select(id => new CurrentAssigneeAndReporterViewModel
            {
                CurrentAssigness = id.AssigneeId,
                Reporter = id.ReporterId,
            });

        return await currentAssigneeAndReporterViewModel.FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetCurrentAssignee(Guid issueId)
    {
        var currentAssigneeId = await _context.IssueDetails
            .Where(id => id.IssueId == issueId)
            .Select(id => id.AssigneeId).FirstOrDefaultAsync();
        return currentAssigneeId;
    }

    public async Task<Guid> GetReporter(Guid issueId)
    {
        var reporterId = await _context.IssueDetails
            .Where(id => id.IssueId == issueId)
            .Select(id => id.ReporterId).FirstOrDefaultAsync();
        return reporterId;
    }

    public async Task UpdateOneColumnForIssueDetail(Guid oldValue, Guid? newValue, NameColumn nameColumn)
    {
        switch (nameColumn)
        {
            case NameColumn.AssigneeId:
                await _context.IssueDetails
                    .Where(i => i.AssigneeId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.AssigneeId, newValue));
                break;
            case NameColumn.ReporterId:
                await _context.IssueDetails
                    .Where(i => i.ReporterId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.ReporterId, newValue));
                break;
        }
    }
}
