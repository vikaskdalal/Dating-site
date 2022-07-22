using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<MessageDto>> GetMessageThread(int senderId, int recipientId);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);

        Task<Message> GetMessage(int id);
    }
}
