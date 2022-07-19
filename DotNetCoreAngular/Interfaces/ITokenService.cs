using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces
{
    public interface ITokenService
    {
        DateTime TokenExpire { get;}

        string CreateToken(User user);
    }
}
