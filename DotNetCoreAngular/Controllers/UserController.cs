using AutoMapper;
using DotNetCoreAngular.DTO;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreAngular.Controllers
{
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
        public async Task<UserDetailDto> Get(string userName)
        {
            var user = await _context.UserRepository.GetByUserNameAsync(userName);
            
            return _mapper.Map<UserDetailDto>(user);
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.UserRepository.GetAll();
        }
    }
}
