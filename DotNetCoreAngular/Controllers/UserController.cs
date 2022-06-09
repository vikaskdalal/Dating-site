using DotNetCoreAngular.DAL;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public UserController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.UserRepository.GetAll();
        }
    }
}
