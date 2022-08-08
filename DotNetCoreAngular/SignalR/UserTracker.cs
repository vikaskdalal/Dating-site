namespace DotNetCoreAngular.SignalR
{
    public class UserTracker
    {
        private static readonly Dictionary<string, HashSet<string>> _onlineUsers =
            new Dictionary<string, HashSet<string>>();

        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;
            lock (_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(username))
                {
                    _onlineUsers[username].Add(connectionId);
                }
                else
                {
                    _onlineUsers.Add(username, new HashSet<string> { connectionId });
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock (_onlineUsers)
            {
                if (!_onlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

                _onlineUsers[username].Remove(connectionId);
                if (_onlineUsers[username].Count == 0)
                {
                    _onlineUsers.Remove(username);
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }

        public string[] GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (_onlineUsers)
            {
                onlineUsers = _onlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return onlineUsers;
        }

        public string[] GetConnectionIdsOfUser(string username)
        {
            lock (_onlineUsers)
            {
                if(_onlineUsers.ContainsKey(username))
                    return _onlineUsers[username].ToArray();
                
                return null;
            }
        }
    }
}
