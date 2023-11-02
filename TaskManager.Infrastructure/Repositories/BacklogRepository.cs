using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
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

        public Task<Backlog?> GetAsync(Guid id)
        {
            var backlog = _context.Backlogs.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
            return backlog;
        }

        public async Task<IReadOnlyCollection<Issue>> GetIssues(Guid backlogId)
        {
            var issues = await _context.Issues
                .AsNoTracking()
                .Where(i => i.BacklogId == backlogId)
                .Include(i => i.Backlog)
                .Include(i => i.IssueType)
                .Include(i => i.IssueDetail)
                .Include(i => i.IssueHistories)
                .Include(i => i.Comments)
                .Include(i => i.Attachments)
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
    }
}
