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
            };

            _context.UserRepository.Add(user);
            await _context.SaveAsync();

            var userDto = new UserDto()
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Name = user.Name
            };

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

            var userDto = new UserDto()
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Name = user.Name
            };

            return Ok(userDto);
        }

        private async Task<bool> UserExistsAsync(string email)
        {
            return await _context.UserRepository.GetByEmailAsync(email) != null;
        }
    }
}
