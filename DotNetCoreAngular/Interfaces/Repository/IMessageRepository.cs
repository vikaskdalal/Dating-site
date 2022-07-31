using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Helpers.Pagination;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<PagedList<MessageDto>> GetMessageThreadAsync(MessageThreadParams messageThreadParams);
        Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);

        Task<Message> GetMessageAsync(int id);
    }
}
