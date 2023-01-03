using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static string GetAuthUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetAuthUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(id);
        }
    }
}
