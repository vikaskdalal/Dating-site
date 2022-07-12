using AutoMapper;
using AutoMapper.QueryableExtensions;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;

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

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = DbSet.OrderByDescending(o => o.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username),
                _ => query.Where(u => u.RecipientUsername ==
                    messageParams.Username && u.DateRead == null)
            };

            return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(int senderId, int recipientId)
        {
            throw new NotImplementedException();
        }
    }
}
