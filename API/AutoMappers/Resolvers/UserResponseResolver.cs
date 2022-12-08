using API.Entities;
using API.Models.Responses;
using AutoMapper;

namespace API.AutoMappers.Resolvers
{
    public class UserResponseResolver : IValueResolver<AppUser, object, UserResponse>
    {
        private readonly IMapper _mapper;

        public UserResponseResolver(IMapper mapper)
        {
            _mapper = mapper;
        }

        public UserResponse Resolve(AppUser source, object destination, UserResponse destMember, ResolutionContext context)
        {
            var userResponse = _mapper.Map<UserResponse>(source);
            return userResponse;
        }
    }
}
