namespace TaskManager.API.Hubs;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> _onlineUsers = new();

    public Task<bool> UserConnected(string userId, string connectionId)
    {
        var isOnline = false;
        lock (_onlineUsers)
        {
            if (_onlineUsers.ContainsKey(userId))
            {
                _onlineUsers[userId].Add(connectionId);
            }
            else
            {
                _onlineUsers.Add(userId, new List<string> { connectionId });
                isOnline = true;
            }
        }
        return Task.FromResult(isOnline);
    }
    public Task<bool> UserDisconnected(string userId, string connectionId)
    {
        var isOffline = false;
        lock (_onlineUsers)
        {
            if (!_onlineUsers.ContainsKey(userId)) return Task.FromResult(isOffline);
            _onlineUsers[userId].Remove(connectionId);
            if (_onlineUsers[userId].Count == 0)
            {
                _onlineUsers.Remove(userId);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onlineUsers;
        lock (_onlineUsers)
        {
            onlineUsers = _onlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
        }

        return Task.FromResult(onlineUsers);
    }

    public Task<List<string>> GetConnectionsForUser(string userId)
    {
        List<string> connectionIds;
        lock (_onlineUsers)
        {
            connectionIds = _onlineUsers.GetValueOrDefault(userId)!;
        }
        return Task.FromResult(connectionIds);
    }
}