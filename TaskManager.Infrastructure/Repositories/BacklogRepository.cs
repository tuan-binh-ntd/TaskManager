using Mapster;
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
            var backlog = _context.Backlogs.SingleOrDefaultAsync(e => e.Id == id);
            return backlog;
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssues(Guid backlogId)
        {
            var issues = await (from i in _context.Issues.Where(e => e.BacklogId == backlogId)
                                join it in _context.IssueTypes on i.IssueTypeId equals it.Id
                                select new IssueViewModel
                                {
                                    Id = i.Id,
                                    Name = i.Name,
                                    Description = i.Description,
                                    CreationTime = i.CreationTime,
                                    CompleteDate = i.CreationTime,
                                    Priority = i.Priority,
                                    Voted = i.Voted,
                                    Watcher = i.Watcher,
                                    StartDate = i.StartDate,
                                    DueDate = i.DueDate,
                                    SprintId = i.SprintId,
                                    ParentId = i.ParentId,
                                    BacklogId = i.BacklogId,
                                    IssueType = it.Adapt<IssueTypeViewModel>()
                                }).ToListAsync();
            return issues.AsReadOnly();
        }

        public async Task<BacklogViewModel> GetBacklog(Guid projectId)
        {
            var backlog = await (from b in _context.Backlogs.Where(e => e.ProjectId == projectId)
                                 select new BacklogViewModel
                                 {
                                     Id = b.Id,
                                     Name = b.Name,
                                 }).FirstOrDefaultAsync();
            return backlog!;
        }
    }
}
