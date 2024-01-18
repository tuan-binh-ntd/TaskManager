namespace TaskManager.Core.Interfaces.Services;

public interface IUserNotificationService
{
    Task<IReadOnlyCollection<UserNotificationViewModel>> GetUserNotificationViewModelsByUserId(Guid userId);
    Task<UserNotificationViewModel?> ReadNotification(Guid id);
    Task<int> GetUnreadUserNotificationNumOfUser(Guid userId);
}
