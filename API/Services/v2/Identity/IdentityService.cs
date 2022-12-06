using API.DataAccess.Interfaces;
using API.DataAccess.Specifications.User;
using API.Entities;
using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Requests.Account;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using ILogger = Serilog.ILogger;

namespace API.Services.v2.Account
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<AppUser> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IdentityService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.Repository<AppUser>();
            _mapper = mapper;
        }

        //public Task<Result<AuthSuccessResponse>> Login(UserLoginRequest request)
        //{
            
        //}

        public async Task<Result<AuthSuccessResponse>> Register(UserRegistrationRequest request)
        {
            _logger.Here().MethoEnterd();
            if(await UserNameExists(request.Username))
            {
                _logger.Here().Information($"User with username {request.Username} already exists");
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.BadRequest, "Username is already in use");
            }

            var user = PopulateUser(request);
            
            // persistance
            _userRepository.Add(user);
            await _unitOfWork.Complete();

            var result = _mapper.Map<AuthSuccessResponse>(user);

            _logger.Here().Information("User registered successfully {@user}", result);
            _logger.Here().MethodExited();
            return Result<AuthSuccessResponse>.Success(result);
        }

        private async Task<bool> UserNameExists(string username)
        {
            var spec = new FindUserByUserNameSpec(username);
            return await _userRepository.GetEntityWithSpec(spec) != null;
        }

        private AppUser PopulateUser(UserRegistrationRequest request)
        {
            using var hmac = new HMACSHA512();
            return new AppUser
            {
                UserName = request.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                LastLogin = null
            };
        }
    }
}
