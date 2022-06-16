using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreAngular.DAL.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbSet.FirstOrDefaultAsync(q => q.Email == email);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await DbSet.FirstOrDefaultAsync(q => q.Username == username);
        }
    }
}
