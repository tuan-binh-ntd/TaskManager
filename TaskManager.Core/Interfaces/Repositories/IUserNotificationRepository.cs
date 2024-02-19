namespace TaskManager.Core.Interfaces.Repositories;

public interface IUserNotificationRepository
{
    Task<IReadOnlyCollection<UserNotificationViewModel>> GetUserNotificationViewModelsByUserIdAsync(Guid userId);
    Task<UserNotificationViewModel?> ToUserNotificationViewModeAsync(Guid id);
    Task<int> GetUnreadUserNotificationNumAsync(Guid userId);
    Task<UserNotification?> GetByIdAsync(Guid id);
    void Update(UserNotification userNotification);
}
