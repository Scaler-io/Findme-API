using API.Entities;

namespace API.Services.Interfaces.v2
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
