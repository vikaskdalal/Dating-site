using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.DAL.Repository
{
    public class LikeRepository : GenericRepository<UserLike>, ILikeRepository
    {
        public LikeRepository(DatabaseContext context) 
            : base(context)
        {
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await DbSet.FindAsync(sourceUserId, likedUserId);
        }

        public IEnumerable<LikeDto> GetUserLikedByMe(int userid)
        {
            var likesQuery = DbSet.AsQueryable();

            var likes = likesQuery.Where(q => q.SourceUserId == userid);
            var users = likes.Select(s => s.LikedUser);

            return users.Select(user => new LikeDto
            {
                Username = user.Username,
                Name = user.Name,
                Age = user.Age,
                City = user.City
            }).ToList();
        }

        public IEnumerable<LikeDto> GetUserWhoLikeMe(int userid)
        {
            var likesQuery = DbSet.AsQueryable();

            var likes = likesQuery.Where(q => q.LikedUserId == userid);
            var users = likes.Select(s => s.SourceUser);

            return users.Select(user => new LikeDto
            {
                Username = user.Username,
                Name = user.Name,
                Age = user.Age,
                City = user.City
            }).ToList();
        }
    }
}
