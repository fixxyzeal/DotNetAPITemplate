using DAL.Models;
using System.Security.Claims;

namespace DotNetTemplate.Helpers
{
    public static class GetAuthenticatedClaimHelper
    {
        public static User GetAuthenticatedClaim(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                throw new ArgumentException("Cannot Identity");
            }

            IEnumerable<Claim> claims = identity.Claims;

            User user = new User()
            {
                UserName = claims.First(x => x.Type == ClaimTypes.Sid).Value,
                Email = claims.First(x => x.Type == ClaimTypes.Email).Value
            };

            return user;
        }
    }
}