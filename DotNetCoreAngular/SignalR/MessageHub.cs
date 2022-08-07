using AutoMapper;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Extensions;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Helpers.Pagination;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DotNetCoreAngular.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private DateTime _cacheExpire = DateTime.UtcNow.AddDays(2);
        private static object _lock = new object();

        public MessageHub(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _context = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();

                var otherUsername = httpContext.Request.Query["user"].ToString();
                var skipMessage = Convert.ToInt32(httpContext.Request.Query["skipMessage"].ToString());
                var takeMessage = Convert.ToInt32(httpContext.Request.Query["takeMessage"].ToString());

                var groupName = GetGroupName(Context.User.GetUsername(), otherUsername);

                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                await AddToGroup(groupName);

                var otherUser = await _context.UserRepository.GetByUsernameAsync(otherUsername);

                var messageThreadParams = new MessageThreadParams();
                messageThreadParams.CurrentUserId = Context.User.GetUserId();
                messageThreadParams.RecipientUserId = otherUser.Id;
                messageThreadParams.SkipMessages = skipMessage;
                messageThreadParams.TakeMessages = takeMessage;

                var messages = await _context.MessageRepository.GetMessageThreadAsync(messageThreadParams);
                messages.TrackMessageThread.FriendUsername = otherUsername;

                if (_context.HasChanges())
                    await _context.SaveAsync();

                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessageThread", messages);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await RemoveConnection(Context.ConnectionId);

            base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername)
                new HubException("Can not send message to yourself");

            var sender = await _context.UserRepository.GetByUsernameAsync(username);
            var recipient = await _context.UserRepository.GetByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null)
                throw new HubException("Not found");

            var message = new Message()
            {
                Sender = sender,
                Recipient = recipient,
                Content = createMessageDto.Content,
            };

            _context.MessageRepository.Add(message);

            if (await _context.SaveAsync())
            {
                var groupName = GetGroupName(sender.Username, recipient.Username);
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        public async Task UserIsTyping(string recipientUsername)
        {
            var username = Context.User.GetUsername();
            var groupName = GetGroupName(username, recipientUsername);

            var connectionIdsOfOtherUsers = GetConnectionIdsInGroupOtherThanCurrentUser(groupName);

            await Clients.Clients(connectionIdsOfOtherUsers).SendAsync("UserIsTyping", username);
        }

        public async Task UserHasStoppedTyping(string recipientUsername)
        {
            var username = Context.User.GetUsername();
            var groupName = GetGroupName(username, recipientUsername);

            var connectionIdsOfOtherUsers = GetConnectionIdsInGroupOtherThanCurrentUser(groupName);

            await Clients.Clients(connectionIdsOfOtherUsers).SendAsync("UserHasStoppedTyping", username);
        }

        public async Task LoadMessageThreadOnScroll(MessageThreadParams messageThreadParams)
        {
            var otherUser = await _context.UserRepository.GetByUsernameAsync(messageThreadParams.RecipientUsername);

            messageThreadParams.CurrentUserId = Context.User.GetUserId();
            messageThreadParams.RecipientUserId = otherUser.Id;

            var messages = await _context.MessageRepository.GetMessageThreadAsync(messageThreadParams);
            messages.TrackMessageThread.FriendUsername = messageThreadParams.RecipientUsername;

            if (_context.HasChanges())
                await _context.SaveAsync();

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessageThreadOnScroll", messages);
        }

        public async Task SendCallNotification(string username, string callType, string callerName)
        {
            var connectionsOfFriend = await _context.ConnectionRepository.GetConnctionsOfUser(username);

            var connectionIds = connectionsOfFriend.Select(s => s.ConnectionId);

            await Clients.Clients(connectionIds).SendAsync("ReceiveCallNotification", new {Context.ConnectionId, notificationType = callType, callerName});
        }

        public async Task SendCallResponse(string callerConnectionId, string callResponse)
        {
            await Clients.Client(callerConnectionId).SendAsync("ReceiveCallNotification", new { Context.ConnectionId, notificationType = callResponse });
        }

        #region Private Methods

        private async Task<bool> AddToGroup(string groupName)
        {
            string cacheKey = SignalRHelper.GetGroupCacheKey(groupName);

            var group = await _context.GroupRepository.GetGroupAsync(groupName);

            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if(group == null)
            {
                group = new Group(groupName);
                _context.GroupRepository.Add(group);
            }

            group.Connections.Add(connection);

            _cacheService.RemoveData(cacheKey);
            _cacheService.SetData(cacheKey, group, _cacheExpire);

            return await _context.SaveAsync();
        }

        private async Task RemoveConnection(string connectionId)
        {
            var connection = await _context.ConnectionRepository.GetByIdAsync(connectionId);

            _context.ConnectionRepository.Delete(connection);

            await _context.SaveAsync();
        }

        private List<string> GetConnectionIdsInGroupOtherThanCurrentUser(string groupName)
        {
            string cacheKey = SignalRHelper.GetGroupCacheKey(groupName);

            var group = _cacheService.GetData<Group>(cacheKey);

            lock (_lock)
            {
                if (group == null)
                {
                    group = _context.GroupRepository.GetGroup(groupName);

                    _cacheService.SetData(cacheKey, group, _cacheExpire);
                }
            }

            return group.Connections.Where(q => q.ConnectionId != Context.ConnectionId).Select(s => s.ConnectionId).ToList();
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        #endregion
    }
}
