using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskManager.API.Extensions;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly IUserNotificationService _userNotificationService;

    public NotificationHub(IUserNotificationService userNotificationService)
    {
        _userNotificationService = userNotificationService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User!.GetUserId();
        // Return notification list
        var notifications = await _userNotificationService.GetUserNotificationViewModelsByUserId(userId);
        await Clients.Caller.SendAsync("NotificationList", notifications);

        // Return unread notification number
        int unreadNotificationNum = await _userNotificationService.GetUnreadUserNotificationNumOfUser(userId);
        await Clients.Caller.SendAsync("UnreadNotificationNum", unreadNotificationNum);
    }

    public async Task ReadNotification(Guid id)
    {
        var res = await _userNotificationService.ReadNotification(id);
        await Clients.Caller.SendAsync("ReadNotification", res);

        int unreadNotificationNum = await _userNotificationService.GetUnreadUserNotificationNumOfUser(id);
        await Clients.Caller.SendAsync("UnreadNotificationNum", unreadNotificationNum);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
