using DotNetCoreAngular.Interfaces.Repository;

namespace DotNetCoreAngular.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        public ILikeRepository LikeRepository { get; }

        int Save();

        Task<int> SaveAsync();
    }
}
