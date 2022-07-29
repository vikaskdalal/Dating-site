using AutoMapper;
using AutoMapper.QueryableExtensions;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Extensions;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreAngular.DAL.Repository
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly IMapper _mapper;

        public MessageRepository(DatabaseContext context, IMapper mapper) 
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var query = DbSet.OrderByDescending(o => o.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername ==
                    messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
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

        public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int currentUserId, int recipientId)
        {
            var messages = await DbSet
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => 
                    m.Recipient.Id == currentUserId && m.RecipientDeleted == false
                    && m.Sender.Id == recipientId
                    ||
                    m.Recipient.Id == recipientId
                    && m.Sender.Id == currentUserId && m.SenderDeleted == false
                )
                .MarkUnreadAsRead(currentUserId)
                .OrderBy(o => o.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return messages;
        }
    }
}
