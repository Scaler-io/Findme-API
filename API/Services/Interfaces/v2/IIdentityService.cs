using API.Models.Core;
using API.Models.Requests.Account;
using API.Models.Responses;

namespace API.Services.Interfaces.v2
{
    public interface IIdentityService
    {
        Task<Result<AuthSuccessResponse>> Register(UserRegistrationRequest request);
        Task<Result<AuthSuccessResponse>> Login(UserLoginRequest request);
    }
}
