using AutoMapper;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Extensions;
using DotNetCoreAngular.Helpers;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreAngular.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UserController(IUnitOfWork context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            this._photoService = photoService;
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
        public async Task<IActionResult> Get([FromQuery] UserParams userParams)
        {
            userParams.CurrentUsername = User.GetUsername();
            var users =  await _context.UserRepository.GetAllUsersWithPhotosAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);

            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserUpdateDto userUpdateDto)
        {
            string? username = User.GetUsername();

            var user = await _context.UserRepository.GetByUsernameAsync(username);

            if (user == null)
                return BadRequest("User not found");

            _mapper.Map(userUpdateDto, user);

            await _context.SaveAsync();

            return Ok(userUpdateDto);
        }

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var user = await _context.UserRepository.GetByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (!user.Photos.Any())
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _context.SaveAsync())
            {
                return Ok(_mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem addding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            var user = await _context.UserRepository.GetByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            
            photo.IsMain = true;

            if (await _context.SaveAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var user = await _context.UserRepository.GetByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _context.SaveAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}
