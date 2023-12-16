using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TaskManager.Core.Entities;

namespace TaskManager.API.Hubs;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _tracker;
    private readonly UserManager<AppUser> _userManager;

    public PresenceHub(
        PresenceTracker tracker,
        UserManager<AppUser> userManager
        )
    {
        _tracker = tracker;
        _userManager = userManager;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        var isOnline = await _tracker.UserConnected(Context!.User!.Identity!.Name!, Context.ConnectionId);

        if (isOnline)
        {
            await Clients.Others.SendAsync("UserIsOnline", Context!.User!.Identity!.Name);
        }

        var currentUsers = await _tracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {

        var isOffline = await _tracker.UserDisconnected(Context!.User!.Identity!.Name!, Context.ConnectionId);
        if (isOffline)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context!.User!.Identity!.Name);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}
