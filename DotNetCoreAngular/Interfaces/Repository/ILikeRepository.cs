using DotNetCoreAngular.DTO;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface ILikeRepository : IGenericRepository<UserLike>
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserid);
        IEnumerable<LikeDto> GetUserWhoLikeMe(int userid);
        IEnumerable<LikeDto> GetUserLikedByMe(int userid);
    }
}
