using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyCollection<Project>> GetAll()
        {
            var projects = await _context.Projects.ToListAsync();
            return projects.AsReadOnly();
        }

        public async Task<Project?> GetById(Guid id)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == id);
            return project;
        }

        public Project Add(Project project)
        {
            return _context.Projects.Add(project).Entity;
        }

        public void Update(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            _context.Projects.Remove(project!);
        }
    }
}
