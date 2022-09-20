using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineChat.Context;
using OnlineChat.Hubs;
using OnlineChat.Models;
using OnlineChat.ViewModels;

namespace OnlineChat.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase {
        private readonly IMapper _mapper;
        private readonly ChatContext _chatContext;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(IMapper mapper,
                                  ChatContext chatContext,
                                  IHubContext<ChatHub> hubContext) {
            _mapper = mapper;
            _chatContext = chatContext;
            _hubContext = hubContext;
        }

        [HttpGet("{id}")]

        public IActionResult Get(int id) {
            var message = _chatContext.Messages
                                      .Include(x => x.FromUser)
                                      .Include(x => x.Room)
                                      .FirstOrDefault(x => x.Id == id);
            if (message == null) {
                return NotFound();
            }

            var messageViewModel = _mapper.Map<MessageViewModel>(message);
            return Ok(messageViewModel);
        }

        [HttpGet("rooms/{roomName}")]
        public async Task<ActionResult<IEnumerable<MessageViewModel>>> GetMessages(string roomName, int skip) {
            var room = await _chatContext.ConversationRooms.FirstOrDefaultAsync(x => x.RoomName == roomName);
            if (room == null) {
                return NotFound();
            }

            var messages = await _chatContext.Messages
                                             .Where(r => r.Room.RoomName == roomName)
                                             .Include(x => x.FromUser)
                                             .Skip(skip)
                                             .Take(20)
                                             .ToListAsync();

            var messagesViewModel = _mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messages);
            return Ok(messagesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post(MessageViewModel mvm, int? replyToId) {
            var user = await _chatContext.Users.FirstOrDefaultAsync(u => u.UserName == mvm.FromUser);
            var room = await _chatContext.ConversationRooms.FirstOrDefaultAsync(r => r.RoomName == mvm.RoomName);
            if (user == null || room == null) {
                return BadRequest();
            }

            var message = _mapper.Map<Message>(mvm);
            message.ReplyToId = replyToId;

            _chatContext.Messages.Add(message);
            await _chatContext.SaveChangesAsync();

            mvm.Id = message.Id;

            if (replyToId != null) {
                var repliedMessage = await _chatContext.Messages.FirstOrDefaultAsync(m => m.Id == replyToId);
                mvm.RepliedContent = repliedMessage?.Content;
            }
            await _hubContext.Clients.Group(mvm.RoomName).SendAsync("newMessage", mvm);
            
            return CreatedAtAction(nameof(Get), new { id = message.Id }, mvm);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(JsonPatchDocument<MessageViewModel> patchMessage, string room, int id) {
            if (patchMessage == null) {
                return BadRequest();
            }
            
            var message = await _chatContext.Messages
                                            .FirstOrDefaultAsync(u => u.Id == id);

            if (message == null ) {
                return BadRequest();
            }

            var messageToEdit = _mapper.Map<MessageViewModel>(message);
            patchMessage.ApplyTo(messageToEdit);
            _mapper.Map(messageToEdit, message);

            _chatContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_chatContext.ChangeTracker.DebugView.LongView);

            await _chatContext.SaveChangesAsync();

            await _hubContext.Clients.Group(room).SendAsync("editMessage", messageToEdit);
            return new ObjectResult(messageToEdit);
        }

        [HttpDelete("{roomName}/{messageId}")]
        public async Task<IActionResult> Delete(string roomName, int messageId) {
            var message = _chatContext.Messages.FirstOrDefault(x => x.Id == messageId);
            if (message == null) {
                NotFound();
            }
            _chatContext.Messages.Remove(message);
            await _chatContext.SaveChangesAsync();

            await _hubContext.Clients.Group(roomName).SendAsync("deleteMessage", message.Id);
            return Ok();
        }
    }
}
