using API.Entities;
using API.Models.Core;
using API.Models.Responses;

namespace API.Services.Interfaces.v2
{
    public interface IUserService
    {
        Task<Result<IReadOnlyList<UserResponse>>> GetUsers();
        Task<Result<UserResponse>> GetUserById(int id);
    }
}
