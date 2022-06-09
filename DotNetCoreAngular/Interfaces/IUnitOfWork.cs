using DotNetCoreAngular.Interfaces.Repository;

namespace DotNetCoreAngular.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        void Save();

        Task SaveAsync();
    }
}
