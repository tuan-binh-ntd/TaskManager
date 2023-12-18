using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskManager.API.Extensions;

namespace TaskManager.API.Hubs;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _tracker;

    public PresenceHub(
        PresenceTracker tracker
        )
    {
        _tracker = tracker;
    }

    public override async Task OnConnectedAsync()
    {
        var isOnline = await _tracker.UserConnected(Context!.User!.GetUserId(), Context.ConnectionId);

        if (isOnline)
        {
            await Clients.Others.SendAsync("UserIsOnline", Context!.User!.GetUserId().ToString());
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {

        var isOffline = await _tracker.UserDisconnected(Context!.User!.GetUserId(), Context.ConnectionId);
        if (isOffline)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context!.User!.GetUserId().ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }
}