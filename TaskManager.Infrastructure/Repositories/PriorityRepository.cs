using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public PriorityRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Priority Add(Priority priority)
        {
            return _context.Priorities.Add(priority).Entity;
        }

        public void AddRange(IReadOnlyCollection<Priority> priorities)
        {
            _context.Priorities.AddRange(priorities);
        }

        public void Delete(Guid id)
        {
            var priority = _context.Priorities.FirstOrDefault(p => p.Id == id);
            _context.Priorities.Remove(priority!);
        }

        public async Task<Priority> GetById(Guid id)
        {
            var priority = await _context.Priorities.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return priority!;
        }

        public async Task<IReadOnlyCollection<Priority>> GetByProjectId(Guid projectId)
        {
            var priorities = await _context.Priorities.AsNoTracking().Where(p => p.ProjectId == projectId && p.ProjectId == null).ToListAsync();
            return priorities!;
        }

        public void Update(Priority priority)
        {
            _context.Entry(priority).State = EntityState.Modified;
        }

        public async Task<Priority> GetNormal()
        {
            var priority = await _context.Priorities.AsNoTracking().FirstOrDefaultAsync(p => p.Name == CoreConstants.LowestName);
            return priority!;
        }
    }
}
