using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.AutoMappers.Resolvers
{
    public class UserRolesResolver : IValueResolver<AppUser, object, List<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRolesResolver(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public List<string> Resolve(AppUser source, object destination, List<string> destMember, ResolutionContext context)
        {
            var roles = _userManager.GetRolesAsync(source).Result;
            var roleList = new List<string>();
            roleList.AddRange(roles.Select(role => role));
            return roleList;
        }
    }
}
