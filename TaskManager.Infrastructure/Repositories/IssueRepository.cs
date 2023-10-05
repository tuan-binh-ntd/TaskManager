using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public IssueRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IssueViewModel Add(Issue issue)
        {
            return _context.Issues.Add(issue).Entity.Adapt<IssueViewModel>();
        }

        public void Delete(Guid id)
        {
            var issue = _context.Issues.SingleOrDefault(e => e.Id == id);
            _context.Issues.Remove(issue!);
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> Gets()
        {
            var issues = await _context.Issues.Select(e => new IssueViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                CreationTime = e.CreationTime,
                CompleteDate = e.CompleteDate,
                Priority = e.Priority,
                Watcher = e.Watcher,
                Voted = e.Voted,
                StartDate = e.StartDate,
                DueDate = e.DueDate,
            }).ToListAsync();
            return issues.AsReadOnly();
        }

        public void Update(Issue issue)
        {
            _context.Entry(issue).State = EntityState.Modified;
        }

        public Issue? Get(Guid id)
        {
            return _context.Issues.SingleOrDefault(e => e.Id == id);
        }

        public async Task<IReadOnlyCollection<Issue>> GetIssueBySprintId(Guid sprintId)
        {
            var issues = await _context.Issues
                .Where(i => i.SprintId == sprintId)
                .Include(i => i.Sprint)
                .Include(i => i.IssueType)
                .Include(i => i.IssueDetail)
                .Include(i => i.IssueHistories)
                .Include(i => i.Comments)
                .Include(i => i.Attachments)
                .ToListAsync();
            return issues.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Issue>> GetIssueByBacklogId(Guid backlogId)
        {
            var issues = await _context.Issues
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
    }
}
