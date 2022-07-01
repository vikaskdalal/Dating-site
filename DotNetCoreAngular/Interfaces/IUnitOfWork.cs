using DotNetCoreAngular.Interfaces.Repository;

namespace DotNetCoreAngular.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        public ILikeRepository LikeRepository { get; }
        public IMessageRepository MessageRepository { get; }

        bool Save();

        Task<bool> SaveAsync();
    }
}
