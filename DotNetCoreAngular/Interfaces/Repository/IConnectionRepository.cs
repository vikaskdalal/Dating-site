using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IConnectionRepository : IGenericRepository<Connection>
    {
        Task<IEnumerable<Connection>> GetConnctionsOfUser(string username);
    }
}
