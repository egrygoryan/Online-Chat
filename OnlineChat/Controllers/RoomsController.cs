using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChat.Context;
using OnlineChat.Models;
using OnlineChat.ViewModels;

namespace OnlineChat.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase {
        private readonly IMapper _mapper;
        private readonly ChatContext _chatContext;

        public RoomsController(IMapper mapper,
                               ChatContext chatContext) {
            _mapper = mapper;
            _chatContext = chatContext;
        }

        [HttpGet("user/{name}")]
        public ActionResult<IEnumerable<ConversationRoomViewModel>> GetRoomsForSpecificUser(string name) {
            var user = _chatContext.Users.FirstOrDefault(x => x.UserName == name);
            if (user == null) { 
                return NotFound();
            }

            var userRooms = _chatContext.ConversationRooms.Where(x => x.Users.Any(u => u.UserName == name)).ToList();
            var roomsViewModel = _mapper.Map<IEnumerable<ConversationRoom>, IEnumerable<ConversationRoomViewModel>>(userRooms);

            return Ok(roomsViewModel);
        }
    }
}
