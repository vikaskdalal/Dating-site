using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
