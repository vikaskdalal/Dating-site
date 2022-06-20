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

        private DateTime _tokenExpire => DateTime.UtcNow.AddMinutes(10);

        public DateTime TokenExpire => _tokenExpire;

        public TokenService(IConfiguration config)
        {
            _configuration = config;
        }
        public string CreateToken(User user)
        {
            var claims = new[] {
                        //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                        new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                        //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        //new Claim("DisplayName", user.DisplayName),
                        //new Claim("UserName", user.UserName),
                        //new Claim("Email", user.Email)
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
