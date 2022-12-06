using API.Entities;
using API.Extensions;
using API.Services.Interfaces.v2;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ILogger = Serilog.ILogger;

namespace API.Services.v2.Identity
{
    public class TokenService: ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public TokenService(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSecretKey"]));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);
            _logger.Here().Information("{@token} token created", jwt);
            return jwt;
        }
    }
}
