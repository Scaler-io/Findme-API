using API.Entities;
using AutoMapper;

namespace API.AutoMappers.Resolvers
{
    public class UserAddressResolver : IValueResolver<AppUser, object, string>
    {
        public string Resolve(AppUser source, object destination, string destMember, ResolutionContext context)
        {
            var address = source.Profile.Address;
            return $"{address.StreetNumber} {address.StreetName} {address.StreetType}, {address.City}, {address.District}, {address.State}, {address.PostCode}";
        }
    }
}
