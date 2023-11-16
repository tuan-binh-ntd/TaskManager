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
            var sprints = await _context.Sprints.AsNoTracking().ProjectToType<SprintViewModel>().ToListAsync();
            return sprints.AsReadOnly();
        }

        public void Update(Sprint sprint)
        {
            _context.Entry(sprint).State = EntityState.Modified;
        }

        public Sprint? Get(Guid id)
        {
            return _context.Sprints.AsNoTracking().SingleOrDefault(e => e.Id == id);
        }

        public async Task<IReadOnlyCollection<Issue>> GetIssues(Guid sprintId)
        {
            var issues = await _context.Issues
                .Where(i => i.SprintId == sprintId)
                .ToListAsync();
            return issues.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<SprintViewModel>> GetSprintByProjectId(Guid projectId)
        {
            var sprints = await _context.Sprints.AsNoTracking().Where(e => e.ProjectId == projectId && e.IsComplete == true).ProjectToType<SprintViewModel>().ToListAsync();
            return sprints.AsReadOnly();
        }
    }
}
