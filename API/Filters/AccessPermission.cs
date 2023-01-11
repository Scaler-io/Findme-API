using API.Models.Constants;
using API.Models.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;

namespace API.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AccessPermission : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
        public AccessPermission(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context != null)
            {
                var presentRoles = _role.Split(',').ToList();
                var user = context.HttpContext.User;
                if (user.Identity.IsAuthenticated)
                {
                    var roles = user.Identities.Select(identity => identity.Claims.Where(c => c.Type == ClaimTypes.Role)).FirstOrDefault();
                    if (roles != null && _role != "*")
                    {
                        foreach (var role in roles)
                        {                           
                            foreach(var item in presentRoles)
                            {
                                if (role.Value == item)
                                {
                                    return;
                                }
                            }
                        }
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Result = new JsonResult("Unathorized")
                        {
                            Value = new ApiResponse(ErrorCodes.Unauthorized, ErrorCodes.Unauthorized)
                        };
                    }
                }
            }
        }
    }
}
