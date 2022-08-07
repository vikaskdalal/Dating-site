using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreAngular.DAL.Repository
{
    public class ConnectionRepository : GenericRepository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(DatabaseContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Connection>> GetConnctionsOfUser(string username)
        {
            return await DbSet.Where(q => q.Username == username).ToListAsync();
        }
    }
}
