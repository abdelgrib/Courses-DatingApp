using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repository;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _repository.GetMessage(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;
            var messagesFromDb = await _repository.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromDb);

            Response.AddPagination(messagesFromDb.CurrentPage, messagesFromDb.PageSize, messagesFromDb.TotalCount, messagesFromDb.TotalPages);
            
            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessagesForUser(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromDb = await _repository.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromDb);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repository.GetUser(userId); /*juste pour le loader en mémoire et l'utiliser pour qu'il soit utiliser par mapper !!! */
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var recipient = await _repository.GetUser(messageForCreationDto.RecipientId);
            if (recipient == null)
                return BadRequest("Could not find user");
            
            messageForCreationDto.SenderId = userId;
            var message = _mapper.Map<Message>(messageForCreationDto);
            _repository.Add<Message>(message);

            if (await _repository.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage", new { id = message.Id}, messageToReturn);
            }
            throw new Exception("Failed to create message");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromDb = await _repository.GetMessage(id);
            if (messageFromDb.SenderId == userId)
                messageFromDb.SenderDeleted = true;

            if (messageFromDb.RecipientId == userId)
                messageFromDb.RecipientDeleted = true;
            
            if (messageFromDb.SenderDeleted && messageFromDb.RecipientDeleted)
                _repository.Delete(messageFromDb);

            if (await _repository.SaveAll())
                return NoContent();
            throw new Exception("Error deleting message");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromDb = await _repository.GetMessage(id);
            if (messageFromDb.RecipientId != userId)
                return Unauthorized();

            messageFromDb.IsRead = true;
            messageFromDb.DateRead = DateTime.Now;

            await _repository.SaveAll();
            return NoContent();
        }
    }
}