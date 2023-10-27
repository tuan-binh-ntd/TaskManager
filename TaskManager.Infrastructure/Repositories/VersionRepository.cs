using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;
using Version = TaskManager.Core.Entities.Version;

namespace TaskManager.Infrastructure.Repositories
{
    public class VersionRepository : IVersionRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public VersionRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
        }

        public Version Add(Version version)
        {
            return _context.Versions.Add(version).Entity;
        }

        public void Delete(Guid id)
        {
            var version = _context.Versions.FirstOrDefault(v => v.Id == id);
            _context.Versions.Remove(version!);
        }

        public async Task<Version> GetById(Guid id)
        {
            var version = await _context.Versions.FirstOrDefaultAsync(v => v.Id == id);
            return version!;
        }

        public async Task<IReadOnlyCollection<Version>> GetByProjectId(Guid projectId)
        {
            var versions = await _context.Versions.Where(e => e.ProjectId == projectId).ToListAsync();
            return versions;
        }

        public void Update(Version version)
        {
            _context.Entry(version).State = EntityState.Modified;
        }

        public void LoadEntitiesRelationship(Version version)
        {
            _context.Entry(version).Collection(i => i.Issues!).Load();
        }
    }
}
