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
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._context = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername)
                return BadRequest("Can not send message to yourself");

            var sender = await _context.UserRepository.GetByUsernameAsync(username);
            var recipient = await _context.UserRepository.GetByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null)
                return NotFound();

            var message = new Message()
            {
                Sender = sender,
                Recipient = recipient,
                Content = createMessageDto.Content,
                SenderUsername = username,
                RecipientUsername = createMessageDto.RecipientUsername
            };

            _context.MessageRepository.Add(message);

            if (await _context.SaveAsync())
                return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _context.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
                messages.TotalCount, messages.TotalPages);

            return Ok(messages);
        }
    }
}
