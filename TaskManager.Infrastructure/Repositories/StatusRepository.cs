using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public StatusRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Status Add(Status status)
        {
            return _context.Statuses.Add(status).Entity;
        }

        public void Delete(Guid id)
        {
            var status = _context.Statuses.FirstOrDefault(x => x.Id == id);
            _context.Statuses.Remove(status!);
        }

        public void Update(Status status)
        {
            _context.Entry(status).State = EntityState.Modified;
        }

        public void AddRange(ICollection<Status> statuses)
        {
            _context.Statuses.AddRange(statuses);
        }
    }
}
