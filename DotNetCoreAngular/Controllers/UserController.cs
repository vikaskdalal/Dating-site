using AutoMapper;
using DotNetCoreAngular.DTO;
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

        [HttpGet("{userName}")]
        public async Task<UserDetailDto> GetUser(string userName)
        {
            var user = await _context.UserRepository.GetByUserNameAsync(userName);
            
            return _mapper.Map<UserDetailDto>(user);
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.UserRepository.GetAll();
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserDetailDto userDetailDto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.UserRepository.GetByUserNameAsync(userId);

            if (user == null)
                return BadRequest("User not found");

            _mapper.Map(userDetailDto, user);

            await _context.SaveAsync();

            return Ok(userDetailDto);


        }
    }
}
