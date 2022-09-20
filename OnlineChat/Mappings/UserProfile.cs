using AutoMapper;
using OnlineChat.Models;
using OnlineChat.ViewModels;

namespace OnlineChat.Mappings {
    public class UserProfile : Profile {
        public UserProfile() {
            CreateMap<User, UserViewModel>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(uv => uv.UserName));
        } 
    }
}
