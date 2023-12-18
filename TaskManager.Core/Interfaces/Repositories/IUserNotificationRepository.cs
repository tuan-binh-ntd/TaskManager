using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IUserNotificationRepository : IRepository<UserNotification>
{
    Task<UserNotificationViewModel> GetByUserId(Guid userId);
    Task<UserNotification?> GetById(Guid id);
    void Add(UserNotification userNotification);
    void Update(UserNotification userNotification);
    void Delete(UserNotification userNotification);
}
