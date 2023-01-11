using API.DataAccess.Interfaces;
using API.DataAccess.Specifications.User;
using API.Entities;
using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Enums;
using API.Models.Requests.Account;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IdentityService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.Repository<AppUser>();
            _mapper = mapper;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result<AuthSuccessResponse>> Login(UserLoginRequest request)
        {
            _logger.Here().MethoEnterd();
            var spec = new FindUserByUserNameSpec(request.Username);
            var user = await _userRepository.GetEntityWithSpec(spec);
            if (user == null)
            {
                _logger.Error("{ErrorCode} - Invalid credential given.", ErrorCodes.Unauthorized);
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.Unauthorized, ErrorMessages.Unauthorized);
            }
            var loginResponse = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!loginResponse.Succeeded)
            {
                _logger.Here().Information($"{ErrorCodes.Operationfailed}: Login attempt failed for username {request.Username}");
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.Operationfailed, "Login attempt failed");
            }
            await UpdateUserLoginTime(user);
            var result = await HandleSuccessResponse(user);
            _logger.Here().Information("Login attempt successfull {@user}", result);
            _logger.Here().MethodExited();
            return Result<AuthSuccessResponse>.Success(result);
        }

        public async Task<Result<AuthSuccessResponse>> Register(UserRegistrationRequest request)
        {
            _logger.Here().MethoEnterd();

            var user = PopulateUser(request);

            // persistance
            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                _logger.Here().Information("{@ErrorCodes}: User registration attempt failed {@Errors}.", ErrorCodes.Operationfailed, identityResult.Errors);
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.BadRequest, "User registration failed.");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Member);

            if (!roleResult.Succeeded)
            {
                _logger.Here().Information("{@ErrorCodes}: Role assignement to user failed {@Errors}.", ErrorCodes.Operationfailed, identityResult.Errors);
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.BadRequest, "User registration failed.");
            }

            var result = await HandleSuccessResponse(user);

            _logger.Here().Information("User registered successfully {@user}", result);
            _logger.Here().MethodExited();
            return Result<AuthSuccessResponse>.Success(result);
        }

        private async Task<AuthSuccessResponse> HandleSuccessResponse(AppUser user)
        {
            var response = _mapper.Map<AuthSuccessResponse>(user);
            response.Token = await _tokenService.CreateToken(user);
            return response;
        }

        private AppUser PopulateUser(UserRegistrationRequest request)
        {
            DateTime.TryParse(request.Profile.DateOfBirth, out var dob);
            Enum.TryParse<Gender>(request.Profile.Gender, out var gender);

            return new AppUser
            {
                UserName = request.Username.ToLower(),               
                Profile = new UserProfile
                {
                    FirstName = request.Profile.FirstName,
                    LastName = request.Profile.LastName,
                    DateOfBirth = dob,
                    KnownAs = request.Profile.KnownAs,
                    Gender = gender,
                    Address = new UserAddress
                    {
                        StreetNumber = request.Address.StreetNumber,
                        StreetName = request.Address.StreetName,
                        StreetType = request.Address.StreetType,
                        City = request.Address.City,
                        District = request.Address.District,
                        State = request.Address.State,
                        PostCode = request.Address.PostCode
                    },
                    CreatedBy = request.Username
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                LastLogin = null
            };
        }
    
        private async Task UpdateUserLoginTime(AppUser user)
        {
            _logger.Here().MethoEnterd();
            user.LastLogin = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            _userRepository.Update(user);
            await _unitOfWork.Complete();
            _logger.Here().Information("User details updated");
            _logger.Here().MethodExited();
        }

        public UserDto GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userName = user.GetAuthUsername();
            var spec = new FindUserByUserNameSpec(userName);
            var userResult = _userRepository.GetEntityWithSpec(spec).Result;
            return _mapper.Map<UserDto>(userResult);
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            _logger.Here().MethoEnterd();
            var user = await _userManager.Users.AnyAsync(u => u.UserName == username);
            if(user)
            {
                _logger.Here().Warning("{@ErrorCode} username already exists.");
                return true;
            }
            _logger.Here().MethodExited();
            return false;
        }

        public async Task<Result<AuthSuccessResponse>> AutoLogin()
        {
            _logger.Here().MethoEnterd();

            var spec = new FindUserByUserNameSpec(GetCurrentUser().Username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            var response = await HandleSuccessResponse(user);

            _logger.Here().MethodExited();
            return Result<AuthSuccessResponse>.Success(response);
        }
    }
}
