using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Alumni.Helpers
{
    public static class UserHelpers
    {
        public static string GetUser(this HttpContext httpContext)
        {
            if (httpContext.User == null) return string.Empty;            
            return httpContext.User.Claims.Single(c => c.Type == Constants.UserContansts.IdClaimType).Value;
        }

        public static string GetRole(this HttpContext httpContext)
        {
            var role = httpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role);

            return role.Value;
        }
    }
}
