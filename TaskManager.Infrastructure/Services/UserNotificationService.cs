namespace TaskManager.Infrastructure.Services;

public class UserNotificationService(
    IUserNotificationRepository userNotificationRepository,
    IUnitOfWork unitOfWork
    )
    : IUserNotificationService
{
    private readonly IUserNotificationRepository _userNotificationRepository = userNotificationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int> GetUnreadUserNotificationNumOfUser(Guid userId)
    {
        var num = await _userNotificationRepository.GetUnreadUserNotificationNumAsync(userId);
        return num;
    }

    public async Task<IReadOnlyCollection<UserNotificationViewModel>> GetUserNotificationViewModelsByUserId(Guid userId)
    {
        var userNotificationViewModels = await _userNotificationRepository.GetUserNotificationViewModelsByUserIdAsync(userId);
        return userNotificationViewModels;
    }

    public async Task<UserNotificationViewModel?> ReadNotification(Guid id)
    {
        var userNotification = await _userNotificationRepository.GetByIdAsync(id) ?? throw new UserNotificationNullException();
        userNotification.Read();
        _userNotificationRepository.Update(userNotification);
        await _unitOfWork.SaveChangesAsync();
        return await _userNotificationRepository.ToUserNotificationViewModeAsync(userNotification.Id);
    }
}
