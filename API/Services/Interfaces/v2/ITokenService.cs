using API.Entities;

namespace API.Services.Interfaces.v2
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
