using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces
{
    public interface ITokenService
    {
        DateTime TokenExpire;

        string CreateToken(User user);
    }
}
