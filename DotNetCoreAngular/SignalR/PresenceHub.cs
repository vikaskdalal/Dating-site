using DotNetCoreAngular.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DotNetCoreAngular.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.GetUsername();

            _tracker.UserConnected(username, Context.ConnectionId);

            var currentUsers = _tracker.GetOnlineUsers();

            await Clients.Client(Context.ConnectionId).SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User.GetUsername();

            _tracker.UserDisconnected(username, Context.ConnectionId);

            var currentUsers = _tracker.GetOnlineUsers();

            await Clients.Others.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
