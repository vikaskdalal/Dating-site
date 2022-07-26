using AutoMapper;
using AutoMapper.QueryableExtensions;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreAngular.DAL.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IMapper _mapper;

        public UserRepository(DatabaseContext context, IMapper mapper) 
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<UserDetailDto>> GetAllUsersWithPhotos(UserParams userParams)
        {
            var query = DbSet.AsQueryable();
            query = query.Where(u => u.Username != userParams.CurrentUsername);

            return await PagedList<UserDetailDto>.CreateAsync(query.ProjectTo<UserDetailDto>(_mapper
                .ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber, userParams.PageSize);
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
