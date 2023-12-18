using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class UserNotificationService : IUserNotificationService
{
    private readonly IUserNotificationRepository _userNotificationRepository;

    public UserNotificationService(IUserNotificationRepository userNotificationRepository)
    {
        _userNotificationRepository = userNotificationRepository;
    }

    public async Task<int> GetUnreadUserNotificationNumOfUser(Guid userId)
    {
        var num = await _userNotificationRepository.GetUnreadUserNotificationNum(userId);
        return num;
    }

    public async Task<IReadOnlyCollection<UserNotificationViewModel>> GetUserNotificationViewModelsByUserId(Guid userId)
    {
        var userNotificationViewModels = await _userNotificationRepository.GetByUserId(userId);
        return userNotificationViewModels;
    }

    public async Task<UserNotificationViewModel?> ReadNotification(Guid id)
    {
        var userNotification = await _userNotificationRepository.GetById(id) ?? throw new UserNotificationNullException();
        userNotification.IsRead = true;
        _userNotificationRepository.Update(userNotification);
        await _userNotificationRepository.UnitOfWork.SaveChangesAsync();
        return await _userNotificationRepository.ToUserNotificationViewMode(userNotification.Id);
    }
}
