using API.Entities;
using API.Models.Core;
using AutoMapper;

namespace API.AutoMappers.Resolvers
{
    public class UserMetadatResolver : IValueResolver<AppUser, object, UserMetadata>
    {
        public UserMetadata Resolve(AppUser source, object destination, UserMetadata destMember, ResolutionContext context)
        {
            return new UserMetadata
            {
                Id = source.Id,
                JoinedOn = source.CreatedAt.HasValue ? source.CreatedAt.Value.ToString("dd-MM-yyyy hh:mm:ss") : string.Empty,
                UpdatedOn = source.UpdatedAt.HasValue ? source.UpdatedAt.Value.ToString("dd-MM-yyyy hh:mm:ss") : string.Empty,
                LastLogin = source.LastLogin.HasValue ? source.LastLogin.Value.ToString("dd-MM-yyyy hh:mm:ss") : string.Empty
            };
        }
    }
}
