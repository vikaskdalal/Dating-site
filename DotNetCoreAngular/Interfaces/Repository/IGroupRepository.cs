using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<Group> GetGroupAsync(string groupName);
        Group GetGroup(string groupName);
    }
}
