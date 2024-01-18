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
        await _tracker.UserConnected(Context!.User!.GetUserId(), Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {

        await _tracker.UserDisconnected(Context!.User!.GetUserId(), Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}