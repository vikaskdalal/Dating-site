using AutoMapper;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Extensions;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetCoreAngular.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{email}")]
        public async Task<UserDetailDto> GetByEmail(string email)
        {
            var user = await _context.UserRepository.GetByEmailAsync(email);
            
            return _mapper.Map<UserDetailDto>(user);
        }

        [HttpGet("getbyusername/{username}")]
        public async Task<UserDetailDto> GetByUsername(string username)
        {
            var user = await _context.UserRepository.GetByUsernameAsync(username);

            return _mapper.Map<UserDetailDto>(user);
        }

        [HttpGet]
        public IEnumerable<UserDetailDto> Get()
        {
            var users =  _context.UserRepository.GetAll();

            return _mapper.Map<IEnumerable<UserDetailDto>>(users);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserDetailDto userDetailDto)
        {
            string? username = User.GetUsername();

            var user = await _context.UserRepository.GetByUsernameAsync(username);

            if (user == null)
                return BadRequest("User not found");

            _mapper.Map(userDetailDto, user);

            await _context.SaveAsync();

            return Ok(userDetailDto);


        }
    }
}
