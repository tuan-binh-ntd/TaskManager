using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IUserNotificationRepository : IRepository<UserNotification>
{
    Task<IReadOnlyCollection<UserNotificationViewModel>> GetByUserId(Guid userId);
    Task<UserNotification?> GetById(Guid id);
    void Add(UserNotification userNotification);
    void Update(UserNotification userNotification);
    void Delete(UserNotification userNotification);
    void AddRange(IReadOnlyCollection<UserNotification> userNotifications);
    Task<UserNotificationViewModel?> ToUserNotificationViewMode(Guid id);
    Task<int> GetUnreadUserNotificationNum(Guid userId);
}
