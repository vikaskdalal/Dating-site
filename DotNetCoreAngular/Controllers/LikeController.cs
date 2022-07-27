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
    public class LikeController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        public LikeController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> AddLike(string username)
        {
            int sourceUserId = User.GetUserId();

            var sourceUser = await _context.UserRepository.GetUserWithLikes(sourceUserId);
            var likedUser = await _context.UserRepository.GetByUsernameAsync(username);

            if(likedUser == null) return BadRequest(new { username = username });

            if(sourceUser.Username == username) return BadRequest("You can not like yourself.");

            var userLike = await _context.LikeRepository.GetUserLike(sourceUserId, likedUser.Id);

            if(userLike != null) return BadRequest("You already like this user.");

            userLike = new UserLike()
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _context.SaveAsync())
                return Ok();

            return BadRequest();
        }

        [HttpGet("like-me")]
        public IActionResult GetUserLikes()
        {
            int sourceUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var likes = _context.LikeRepository.GetUserWhoLikeMe(sourceUserId);

            return Ok(likes);
        }

        [HttpGet("liked-by-me")]
        public IActionResult GetUserLikedByMe()
        {
            int sourceUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var likes = _context.LikeRepository.GetUserLikedByMe(sourceUserId);

            return Ok(likes);
        }

        [HttpDelete("remove-like/{username}")]
        public async Task<IActionResult> RemoveLike(string username)
        {
            int sourceUserId = User.GetUserId();

            var sourceUser = await _context.UserRepository.GetUserWithLikes(sourceUserId);
            var likedUser = await _context.UserRepository.GetByUsernameAsync(username);

            if (likedUser == null) return BadRequest(new { username = username });

            var userLike = await _context.LikeRepository.GetUserLike(sourceUserId, likedUser.Id);

            if(userLike == null) return BadRequest(new { username = username });

            _context.LikeRepository.Delete(userLike);

            if (await _context.SaveAsync())
                return Ok();

            return BadRequest(new { username = username }); 
        }
    }
}
