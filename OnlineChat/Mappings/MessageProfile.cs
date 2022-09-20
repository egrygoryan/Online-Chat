using AutoMapper;
using OnlineChat.Context;
using OnlineChat.Models;
using OnlineChat.ViewModels;

namespace OnlineChat.Mappings {
    public class MessageProfile : Profile {
        private readonly ChatContext _chatContext;
        public MessageProfile() {
            CreateMap<Message, MessageViewModel>()
                .ForMember(mvm => mvm.Id, opt => opt.MapFrom(m => m.Id))
                .ForMember(mvm => mvm.Content, opt => opt.MapFrom(m => m.Content))
                .ForMember(mvm => mvm.FromUser, opt => opt.MapFrom(m => m.FromUser.UserName))
                .ForMember(mvm => mvm.RoomName, opt => opt.MapFrom(m => m.Room.RoomName))
                .ForMember(mvm => mvm.RepliedContent, opt => opt.MapFrom(m => _chatContext.Messages.Where(x => x.Id == m.ReplyToId).Select(s => s.Content).SingleOrDefault()));
            CreateMap<MessageViewModel, Message>()
                .ForMember(m => m.Content, opt => opt.MapFrom(mvm => mvm.Content))
                .ForMember(m => m.FromUser, opt => opt.MapFrom(mvm => _chatContext.Users.FirstOrDefault(u => u.UserName == mvm.FromUser)))
                .ForMember(m => m.Room, opt => opt.MapFrom(mvm => _chatContext.ConversationRooms.FirstOrDefault(r => r.RoomName == mvm.RoomName)));
        }
        public MessageProfile(ChatContext chatContext) : this() {
            _chatContext = chatContext;
        }
    }
}
