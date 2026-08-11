using System.Security.Claims;
using System.Security.Principal;

namespace Clinic.Mvc.Extensions
{
    public static class IdentityExtension
    {
        public static int GetUserId(this ClaimsPrincipal? claimsPrincipal)
        {
            var data = claimsPrincipal?.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
            return data != null ? Convert.ToInt32(data.Value) : 0;
        }

        public static int GetUserId(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;
            return user.GetUserId();
        }
    }
}
