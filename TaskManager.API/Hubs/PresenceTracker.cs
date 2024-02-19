namespace TaskManager.API.Hubs;

public class PresenceTracker
{
    private static readonly Dictionary<Guid, List<string>> _onlineUsers = [];

    public Task<bool> UserConnected(Guid userId, string connectionId)
    {
        var isOnline = false;
        lock (_onlineUsers)
        {
            if (_onlineUsers.TryGetValue(userId, out var value))
            {
                value.Add(connectionId);
            }
            else
            {
                _onlineUsers.Add(userId, [connectionId]);
                isOnline = true;
            }
        }
        return Task.FromResult(isOnline);
    }
    public Task<bool> UserDisconnected(Guid userId, string connectionId)
    {
        var isOffline = false;
        lock (_onlineUsers)
        {
            if (!_onlineUsers.TryGetValue(userId, out var value)) return Task.FromResult(isOffline);
            value.Remove(connectionId);
            if (value.Count == 0)
            {
                _onlineUsers.Remove(userId);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }

    public Task<Guid[]> GetOnlineUsers()
    {
        Guid[] onlineUsers;
        lock (_onlineUsers)
        {
            onlineUsers = _onlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
        }

        return Task.FromResult(onlineUsers);
    }

    public Task<List<string>> GetConnectionsForUser(Guid userId)
    {
        List<string> connectionIds;
        lock (_onlineUsers)
        {
            connectionIds = _onlineUsers.GetValueOrDefault(userId)!;
        }
        return Task.FromResult(connectionIds);
    }

    public IReadOnlyCollection<string> GetConnectionsForUserIds(IReadOnlyCollection<Guid> userIds)
    {
        List<string> connectionIds = [];
        lock (_onlineUsers)
        {
            foreach (var userId in userIds)
            {
                var list = _onlineUsers.GetValueOrDefault(userId);
                if (list is not null && list.Count > 0)
                {
                    connectionIds.AddRange(list!);
                }
            }
        }
        return connectionIds.AsReadOnly();
    }
}