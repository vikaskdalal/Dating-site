using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.DAL.Repository
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) 
            : base(context)
        {
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(int senderId, int recipientId)
        {
            throw new NotImplementedException();
        }
    }
}
