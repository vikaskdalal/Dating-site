using DotNetCoreAngular.Interfaces.Repository;

namespace DotNetCoreAngular.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        public ILikeRepository LikeRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IConnectionRepository ConnectionRepository { get; }

        bool Save();

        Task<bool> SaveAsync();
    }
}
