using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetCoreAngular.DAL;
using DotNetCoreAngular.DTO;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using System.Security.Cryptography;
using System.Text;

namespace DotNetCoreAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ITokenService _tokenService;

        public AccountController(IUnitOfWork context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
        {
            if(await UserExistsAsync(registerDto.Email)) 
                return BadRequest("Email is already registered.");

            var hmac = new HMACSHA512();

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                DateOfBirth = registerDto.DateOfBirth,
                Name = registerDto.Name,
                Username = CreateUsername(registerDto.Email)
            };

            _context.UserRepository.Add(user);
            await _context.SaveAsync();

            var token = _tokenService.CreateToken(user);

            var userDto = new UserDto(token, user.Name, user.Username, user.Email);

            return CreatedAtAction(nameof(RegisterAsync), userDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.UserRepository.GetByEmailAsync(loginDto.Email);

            if (user == null) 
                return Unauthorized("Invalid email or password");

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid email or password");
            }

            var token = _tokenService.CreateToken(user);

            var userDto = new UserDto(token, user.Name, user.Username, user.Email);

            return Ok(userDto);
        }

        private async Task<bool> UserExistsAsync(string email)
        {
            return await _context.UserRepository.GetByEmailAsync(email) != null;
        }

        private string CreateUsername(string email)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(email));
                var res = Convert.ToBase64String(hash);
                return res.Substring(0, 7);
            }
        }
    }
}
