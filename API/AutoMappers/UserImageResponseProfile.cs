using API.Entities;
using API.Models.Responses;
using AutoMapper;

namespace API.AutoMappers
{
    public class UserImageResponseProfile: Profile
    {
        public UserImageResponseProfile()
        {
            CreateMap<UserImageResponse, UserImage>();
        }
    }
}
