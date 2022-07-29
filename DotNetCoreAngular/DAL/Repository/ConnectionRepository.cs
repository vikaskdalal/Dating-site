using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.DAL.Repository
{
    public class ConnectionRepository : GenericRepository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}
