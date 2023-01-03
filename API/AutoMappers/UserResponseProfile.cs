using API.AutoMappers.Resolvers;
using API.Entities;
using API.Models.Core;
using API.Models.Responses;
using AutoMapper;

namespace API.AutoMappers
{
    public class UserResponseProfile: Profile
    {
        public UserResponseProfile()
        {
            CreateMap<AppUser, UserResponse>()
                .ForMember(d => d.Profile, o => o.MapFrom(s => s.Profile))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Profile.Address))
                .ForMember(d => d.Photos, o => o.MapFrom(s => s.Profile.Photos))
                .ForMember(d => d.Metadata, o => o.MapFrom<UserMetadatResolver>());

            CreateMap<UserProfile, UserProfileResponse>()
                .ForMember(d => d.Age, o => o.MapFrom(d => d.GetAge()))
                .ForMember(d => d.Gender, o => o.MapFrom(d => d.Gender.ToString()));

            CreateMap<UserAddress, UserAddressResponse>();

            CreateMap<UserImage, UserImageResponse>();

            CreateMap<AppUser, UserDto>()
                .ForMember(d => d.KnownAs, o => o.MapFrom(d => d.Profile.KnownAs))
                .ForMember(d => d.Address, o => o.MapFrom<UserAddressResolver>())
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Profile.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.Profile.LastName)).ReverseMap();
        }
    }
}
