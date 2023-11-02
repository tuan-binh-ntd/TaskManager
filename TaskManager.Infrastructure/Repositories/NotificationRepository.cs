using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Notification Add(Notification notification)
        {
            return _context.Notifications.Add(notification).Entity;
        }

        public void Delete(Guid id)
        {
            var notification = _context.Notifications.FirstOrDefault(o => o.Id == id);
            _context.Notifications.Remove(notification!);
        }

        public async Task<Notification> GetById(Guid id)
        {
            var notification = await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
            return notification!;
        }

        public async Task<IReadOnlyCollection<Notification>> GetByUserId(Guid userId)
        {
            var notifications = await _context.Notifications.AsNoTracking().Where(o => o.CreatorUserId == userId).ToListAsync();
            return notifications.AsReadOnly();
        }

        public void Update(Notification notification)
        {
            _context.Entry(notification).State = EntityState.Modified;
        }
    }
}
