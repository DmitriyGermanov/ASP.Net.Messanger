using AutoMapper;
using MessagingService.Dtos;
using MessagingService.Models;
using MessagingService.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessagingService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MessageController(IMessageRepository messageRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMessageRepository _messageRepository = messageRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("messages/getAllMessages")]
        [Authorize]
        public ActionResult<IEnumerable<Message>> GetUnreceivedMessages()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID.");

            var messages = _messageRepository.GetMessagesByUser(userId);
            return Ok(messages);
        }
        [HttpPost("messages/addMessage")]
        [Authorize]
        public ActionResult SendMessage([FromBody] SendMessageRequestDTO messageToSend)
        {
            if (User == null || User.Identity == null)
                throw new ArgumentNullException(nameof(User));

            if (!User.Identity.IsAuthenticated)
                return Unauthorized("User is not authenticated.");

            var senderIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (senderIdClaim == null)
                return Unauthorized("User ID claim is missing.");

            if (!Guid.TryParse(senderIdClaim, out var senderId))
                return BadRequest("Invalid user ID format.");

            Message message = CreateMessage(messageToSend, senderId);

            _messageRepository.AddMessage(message);

            return CreatedAtAction(nameof(SendMessage), new { id = message.Id }, message);

        }

        private Message CreateMessage(SendMessageRequestDTO messageToSend, Guid senderID)
        {
            var message = _mapper.Map<Message>(messageToSend);

            message.Id = new Guid();
            message.SenderId = senderID;
            message.SentAt = DateTime.UtcNow;
            message.IsReceived = false;
            return message;
        }
    }
}
