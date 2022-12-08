using API.AutoMappers.Resolvers;
using API.Entities;
using API.Models.Responses;
using AutoMapper;

namespace API.AutoMappers
{
    public class AuthResponseProfile: Profile
    {
        public AuthResponseProfile()
        {
            CreateMap<AppUser, AuthSuccessResponse>()
                .ForMember(d => d.User, o => o.MapFrom<UserResponseResolver>());
        }
    }
}
