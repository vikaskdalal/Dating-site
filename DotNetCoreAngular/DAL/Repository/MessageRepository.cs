using AutoMapper;
using AutoMapper.QueryableExtensions;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Extensions;
using DotNetCoreAngular.Helpers.Pagination;
using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreAngular.DAL.Repository
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        public MessageRepository(DatabaseContext context, IMapper mapper) 
            : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var query = DbSet.OrderByDescending(o => o.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientId ==
                    messageParams.UserId && u.RecipientDeleted == false && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            return await DbSet
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MessageThreadDto> GetMessageThreadAsync(MessageThreadParams messageThreadParams)
        {
            var query = DbSet.AsQueryable()
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m =>
                    m.Recipient.Id == messageThreadParams.CurrentUserId && m.RecipientDeleted == false
                    && m.Sender.Id == messageThreadParams.RecipientUserId
                    ||
                    m.Recipient.Id == messageThreadParams.RecipientUserId
                    && m.Sender.Id == messageThreadParams.CurrentUserId && m.SenderDeleted == false
                )
                .MarkUnreadAsRead(messageThreadParams.CurrentUserId)
                .OrderByDescending(o => o.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            var totalCount = await query.CountAsync();
            
            var result = await query.Skip(messageThreadParams.SkipMessages).Take(messageThreadParams.TakeMessages).ToListAsync();
            result = result.OrderBy(o => o.MessageSent).ToList();

            int totalMessagesLoaded = GetTotalMessagesLoaded(totalCount, messageThreadParams.SkipMessages, messageThreadParams.TakeMessages);

            return new MessageThreadDto(totalCount, totalMessagesLoaded, result);
        }

        public void ClearUserChat(int senderId, int recipientId)
        {
            var messages = DbSet.Where(m =>
                    m.Recipient.Id == senderId && m.RecipientDeleted == false
                    && m.Sender.Id == recipientId
                    ||
                    m.Recipient.Id == recipientId
                    && m.Sender.Id == senderId && m.SenderDeleted == false
                );

            foreach (var message in messages.Where(q => q.SenderId == senderId))
            {
                if (message.RecipientDeleted)
                    DbSet.Remove(message);
                else
                    message.SenderDeleted = true;
            }
                

            foreach (var message in messages.Where(q => q.RecipientId == senderId))
            {
                if (message.SenderDeleted)
                    DbSet.Remove(message);
                else
                    message.RecipientDeleted = true;
            }
        }

        private int GetTotalMessagesLoaded(int totalMessages, int skippedMessages, int takeMessages)
        {
            return skippedMessages + takeMessages >= totalMessages ? totalMessages : skippedMessages + takeMessages;
        }
    }
}
