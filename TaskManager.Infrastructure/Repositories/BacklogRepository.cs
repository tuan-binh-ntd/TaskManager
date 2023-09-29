using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
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
    }
}
