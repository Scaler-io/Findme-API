using API.DataAccess.Interfaces;
using API.DataAccess.Specifications.User;
using API.Entities;
using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Requests.User;
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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uniitOfWork;

        public UserService(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _uniitOfWork = unitOfWork;
            _userRepository = unitOfWork.Repository<AppUser>();
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<UserResponse>> GetUserById(int id)
        {
            _logger.Here().MethoEnterd();
            var spec = new FindUserWithProfileInfoSpec(id);
            var user = await _userRepository.GetEntityWithSpec(spec); 
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
            var spec = new GetAllUsersWithProfileInfoSpec();
            var users = await _userRepository.ListAsync(spec);
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

        public async Task<Result<UserResponse>> GetUserByUserName(string username)
        {
            _logger.Here().MethoEnterd();

            var spec = new FindUserByUserNameSpec(username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            if (user == null)
            {
                _logger.Here().Information("No user was found in database with {@UserName}", username);
            }

            var result = _mapper.Map<UserResponse>(user);

            _logger.Here().MethodExited();
            return Result<UserResponse>.Success(result);
        }

        public async Task<Result<UserResponse>> UpdateUserInfo(UserUpdateRequest request, UserDto currentUser)
        {
            _logger.Here().MethoEnterd();
            var spec = new FindUserByUserNameSpec(currentUser.Username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            if(user == null)
            {
                _logger.Here().Warning("{@ErrorCode} user not found with {@userName}", ErrorCodes.NotFound, currentUser.Username);
                return Result<UserResponse>.Fail(ErrorCodes.NotFound);
            }

            _mapper.Map(request, user.Profile);
            user.UpdatedAt = DateTime.UtcNow;
            user.Profile.UpdatedBy = user.UserName;

            _userRepository.Update(user);

            if (await _uniitOfWork.Complete() < 1)
            {
                _logger.Here().Error("{@ErrorCode}Failed to update user info", ErrorCodes.Operationfailed);
                return Result<UserResponse>.Fail(ErrorCodes.Operationfailed);
            }
            var result = await GetUserByUserName(currentUser.Username);

            _logger.Here().Information("user info updated {@user}", result);
            _logger.Here().MethodExited();
            return result;
        }
    }
}
