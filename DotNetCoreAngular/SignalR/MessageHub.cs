using AutoMapper;
using DotNetCoreAngular.Common;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Extensions;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using Microsoft.AspNetCore.SignalR;

namespace DotNetCoreAngular.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private DateTime _cacheExpire = DateTime.UtcNow.AddDays(2);

        public MessageHub(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            this._context = unitOfWork;
            this._mapper = mapper;
            _cacheService = cacheService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var otherUser = httpContext.Request.Query["user"].ToString();

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await AddToGroup(groupName);

            var otherUserId = await _context.UserRepository.GetByUsernameAsync(otherUser);
            var messages = await _context.MessageRepository.GetMessageThread(Context.User.GetUserId(), otherUserId.Id);

            Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
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
                SenderUsername = username,
                RecipientUsername = createMessageDto.RecipientUsername
            };

            _context.MessageRepository.Add(message);

            if (await _context.SaveAsync())
            {
                var groupName = GetGroupName(sender.Username, recipient.Username);
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            string cacheKey = $"{InMemoryCacheKeys.SIGNALR_GROUP}_{groupName}";

            var group = await _context.GroupRepository.GetGroup(groupName);

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

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
