using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class UserNotificationRepository : IUserNotificationRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public UserNotificationRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(UserNotification userNotification)
    {
        _context.Add(userNotification);
    }

    public void Delete(UserNotification userNotification)
    {
        _context.Remove(userNotification);
    }

    public async Task<UserNotification?> GetById(Guid id)
    {
        var userNotification = await _context.UserNotifications.Where(un => un.Id == id).FirstOrDefaultAsync();
        return userNotification;
    }

    public Task<UserNotificationViewModel> GetByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public void Update(UserNotification userNotification)
    {
        _context.Entry(userNotification).State = EntityState.Modified;
    }
}
