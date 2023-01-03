using API.Entities;
using API.Models.Requests.User;
using AutoMapper;

namespace API.AutoMappers
{
    public class UserUpdateRequestProfile: Profile
    {
        public UserUpdateRequestProfile()
        {
            CreateMap<UserUpdateRequest, UserProfile>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ReverseMap();
        }
    }
}
