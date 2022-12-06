using API.AutoMappers.Resolvers;
using API.Entities;
using API.Models.Responses;
using AutoMapper;

namespace API.AutoMappers
{
    public class UserResponseProfile: Profile
    {
        public UserResponseProfile()
        {
            CreateMap<AppUser, UserResponse>()
                .ForMember(d => d.Metadata, o => o.MapFrom<UserMetadatResolver>());
        }
    }
}
