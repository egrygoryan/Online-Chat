using AutoMapper;
using OnlineChat.Models;
using OnlineChat.ViewModels;

namespace OnlineChat.Mappings {
    public class ConversationRoomProfile : Profile {
        public ConversationRoomProfile() {
            CreateMap<ConversationRoom, ConversationRoomViewModel>()
                .ForMember(cr => cr.Id, opt => opt.MapFrom(crv => crv.Id))
                .ForMember(cr => cr.RoomName, opt => opt.MapFrom(crv => crv.RoomName));
        }
    }
}
