using API.DataAccess.Interfaces;
using API.Entities;
using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using AutoMapper;
using ILogger = Serilog.ILogger;

namespace API.Services.v2.Users
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<AppUser> _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
        {
            _userRepository = unitOfWork.Repository<AppUser>();
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<UserResponse>> GetUserById(int id)
        {
            _logger.Here().MethoEnterd();
            var user = await _userRepository.GetByIdAsync(id);
            if(user == null)
            {
                _logger.Here().Error($"No user was found with id {id}");
                return Result<UserResponse>.Fail(ErrorCodes.NotFound, ErrorMessages.NotFound);
            }

            var result = _mapper.Map<UserResponse>(user);
            _logger.Here().Information("user details fetched - {@user}", result);
            _logger.Here().MethodExited();
            return Result<UserResponse>.Success(result);
        }

        public async Task<Result<IReadOnlyList<UserResponse>>> GetUsers()
        {
            _logger.Here().MethoEnterd();
            var users = await _userRepository.ListAllAsync();
            if (users == null)
            {
                _logger.Here().Error($"No users found in database");
                return Result<IReadOnlyList<UserResponse>>.Fail(ErrorCodes.NotFound, ErrorMessages.NotFound);
            }

            var result = _mapper.Map<IReadOnlyList<UserResponse>>(users);

            _logger.Here().Information($"user list fetched - [{string.Join(",", users.Select(u => u.UserName))}]");
            _logger.Here().MethodExited();
            return Result<IReadOnlyList<UserResponse>>.Success(result);
        }
    }
}
