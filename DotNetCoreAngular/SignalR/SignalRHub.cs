using DotNetCoreAngular.Extensions;
using DotNetCoreAngular.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DotNetCoreAngular.SignalR
{
    [Authorize]
    public class SignalRHub : Hub
    {
        private readonly UserTracker _tracker;
        private readonly IUnitOfWork _context;
        public SignalRHub(UserTracker tracker, IUnitOfWork unitOfWork)
        {
            _tracker = tracker;
            _context = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.GetUsername();

            _tracker.UserConnected(username, Context.ConnectionId);

            var currentUsers = _tracker.GetOnlineUsers();

            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User.GetUsername();

            _tracker.UserDisconnected(username, Context.ConnectionId);

            var currentUsers = _tracker.GetOnlineUsers();

            await Clients.Others.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendCallNotification(string username, string callType)
        {
            var connectionsIdsOfFriend = _tracker.GetConnectionIdsOfUser(username);

            if(connectionsIdsOfFriend != null)
            {
                await Clients.Clients(connectionsIdsOfFriend).SendAsync("ReceiveCallNotification", 
                    new { Context.ConnectionId, notificationType = callType, callerUsername = Context.User.GetUsername() });
            }
        }

        public async Task SendCallResponse(string callerConnectionId, string response, dynamic data)
        {
            await Clients.Client(callerConnectionId).SendAsync("ReceiveCallNotification", 
                new { Context.ConnectionId, notificationType = response, data, callerUsername = Context.User.GetUsername() });
        }
    }
}
