using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<MessageDto>> GetMessageThread(int senderId, int recipientId);
    }
}
