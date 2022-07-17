using Microsoft.IdentityModel.Tokens;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCoreAngular.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        private DateTime _tokenExpire => DateTime.UtcNow.AddMinutes(60);

        public DateTime TokenExpire => _tokenExpire;

        public TokenService(IConfiguration config)
        {
            _configuration = config;
        }
        public string CreateToken(User user)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                        new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: _tokenExpire,
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
