using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Helpers.Pagination;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int senderId, int recipientId);
        Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);

        Task<Message> GetMessageAsync(int id);
    }
}
