using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class SprintRepository : ISprintRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public SprintRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public SprintViewModel Add(Sprint sprint)
        {
            return _context.Sprints.Add(sprint).Entity.Adapt<SprintViewModel>();
        }

        public void Delete(Guid id)
        {
            var sprint = _context.Sprints.SingleOrDefault(e => e.Id == id);
            _context.Sprints.Remove(sprint!);
        }

        public async Task<IReadOnlyCollection<SprintViewModel>> Gets()
        {
            var sprints = await _context.Sprints.ProjectToType<SprintViewModel>().ToListAsync();
            return sprints.AsReadOnly();
        }

        public void Update(Sprint sprint)
        {
            _context.Entry(sprint).State = EntityState.Modified;
        }

        public Sprint? Get(Guid id)
        {
            return _context.Sprints.SingleOrDefault(e => e.Id == id);
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssues(Guid sprintId)
        {
            var issues = await (from i in _context.Issues.Where(e => e.SprintId == sprintId)
                                join it in _context.IssueTypes on i.IssueTypeId equals it.Id
                                select new IssueViewModel
                                {
                                    Id = i.Id,
                                    Name = i.Name,
                                    Description = i.Description,
                                    CreationTime = i.CreationTime,
                                    CompleteDate    = i.CreationTime,
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

        public async Task<IReadOnlyCollection<SprintViewModel>> GetSprintByProjectId(Guid projectId)
        {
            var sprints = await _context.Sprints.Where(e => e.ProjectId == projectId).ProjectToType<SprintViewModel>().ToListAsync();
            return sprints.AsReadOnly();    
        }
    }
}
