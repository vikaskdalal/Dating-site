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
            if(await UserExistsAsync(registerDto.Username)) 
                return BadRequest("Username is already taken.");

            var hmac = new HMACSHA512();

            var user = new User
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                DateOfBirth = registerDto.DateOfBirth
            };

            _context.UserRepository.Add(user);
            await _context.SaveAsync();

            var userDto = new UserDto()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return CreatedAtAction(nameof(RegisterAsync), userDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.UserRepository.GetByUserNameAsync(loginDto.Username);

            if (user == null) 
                return Unauthorized("Invalid UserName");

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            var userDto = new UserDto()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }

        private async Task<bool> UserExistsAsync(string username)
        {
            return await _context.UserRepository.GetByUserNameAsync(username) != null;
        }
    }
}
