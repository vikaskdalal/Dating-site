using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreAngular.DAL.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPhotos()
        {
            return await DbSet.Include(p => p.Photos).ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbSet.Include(p => p.Photos).FirstOrDefaultAsync(q => q.Email == email);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await DbSet.Include(p => p.Photos).FirstOrDefaultAsync(q => q.Username == username);
        }

        public async Task<User> GetUserWithLikes(int userid)
        {
            return await DbSet
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userid);
        }
       
    }
}
