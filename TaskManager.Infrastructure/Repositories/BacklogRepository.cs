using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class BacklogRepository : IBacklogRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public BacklogRepository(
        AppDbContext context
        )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Backlog Add(Backlog backlog)
    {
        return _context.Add(backlog).Entity;
    }

    public async Task<Backlog?> GetAsync(Guid id)
    {
        var backlog = await _context.Backlogs.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        return backlog;
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssues(Guid backlogId)
    {
        var projectId = await _context.Backlogs.AsNoTracking().Where(b => b.Id == backlogId).Select(b => b.ProjectId).FirstOrDefaultAsync();

        var subtaskTypeId = await _context.IssueTypes.AsNoTracking().Where(it => it.ProjectId == projectId && it.Name == CoreConstants.SubTaskName).Select(it => it.Id).FirstOrDefaultAsync();

        var issues = await _context.Issues
            .Where(i => i.BacklogId == backlogId && i.IssueTypeId != subtaskTypeId)
            .ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<BacklogViewModel> GetBacklog(Guid projectId)
    {
        var backlog = await (from b in _context.Backlogs.AsNoTracking().Where(e => e.ProjectId == projectId)
                             select new BacklogViewModel
                             {
                                 Id = b.Id,
                                 Name = b.Name,
                             }).FirstOrDefaultAsync();
        return backlog!;
    }

    public async Task<Backlog?> GetByProjectId(Guid projectId)
    {
        var backlog = await _context.Backlogs.Where(b => b.ProjectId == projectId).SingleOrDefaultAsync();
        return backlog;
    }
}
