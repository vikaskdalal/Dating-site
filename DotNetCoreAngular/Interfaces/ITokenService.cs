using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
