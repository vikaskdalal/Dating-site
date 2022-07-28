using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreAngular.DAL.Repository
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(DatabaseContext context)
            : base(context)
        {
        }

        public Task<Group> GetGroup(string groupName)
        {
            return DbSet
                .Include(c => c.Connections)
                .FirstOrDefaultAsync(q => q.Name == groupName);
        }
    }
}
