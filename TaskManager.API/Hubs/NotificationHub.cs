﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskManager.API.Extensions;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly IUserNotificationService _userNotificationService;
    private readonly PresenceTracker _tracker;

    public NotificationHub(
        IUserNotificationService userNotificationService,
        PresenceTracker tracker
        )
    {
        _userNotificationService = userNotificationService;
        _tracker = tracker;
    }

    public override async Task OnConnectedAsync()
    {
        await _tracker.UserConnected(Context!.User!.GetUserId(), Context.ConnectionId);

        var userId = Context.User!.GetUserId();
        // Return notification list
        var notifications = await _userNotificationService.GetUserNotificationViewModelsByUserId(userId);
        await Clients.Caller.SendAsync("NotificationList", notifications);

        // Return unread notification number
        int unreadNotificationNum = await _userNotificationService.GetUnreadUserNotificationNumOfUser(userId);
        await Clients.Caller.SendAsync("UnreadNotificationNum", unreadNotificationNum);

        await base.OnConnectedAsync();
    }

    public async Task ReadNotification(Guid id)
    {
        var userId = Context.User!.GetUserId();

        var res = await _userNotificationService.ReadNotification(id);
        await Clients.Caller.SendAsync("ReadNotification", res);

        int unreadNotificationNum = await _userNotificationService.GetUnreadUserNotificationNumOfUser(userId);
        await Clients.Caller.SendAsync("UnreadNotificationNum", unreadNotificationNum);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _tracker.UserDisconnected(Context!.User!.GetUserId(), Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
