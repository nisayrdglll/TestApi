using System.Security.Claims;

namespace Helpers.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetName(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.Name);
        }

        public static string GetFirstName(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.FirstName);
        }

        public static string GetLastName(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.LastName);
        }

        public static string GetEmail(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.Email);
        }

        public static string GetUid(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.Id);
        }

        public static string GetDepartmentId(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.DepartmentId);
        }

        public static string GetReferenceResources(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.Id);
        }

        public static string GetProfilePicture(this ClaimsPrincipal current)
        {
            return GetClaimValue(current, Enums.UserEnums.Picture);
        }

        public static DateTime GetExpirationDateTime(this ClaimsPrincipal current)
        {
            var timestamp = long.Parse(GetClaimValue(current, "exp"));
            var datetime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
            return datetime;
        }

        private static string GetClaimValue(ClaimsPrincipal principal, string type)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type.Equals(type));
            if (claim != null)
            {
                if (!string.IsNullOrEmpty(claim.Value))
                {
                    return claim.Value;
                }

            }

            return "";
        }

        public static List<string> GetRoles(this ClaimsPrincipal current)
        {
            try
            {
                if (current == null)
                {
                    return new List<string>();
                }
                var roleList = current.Claims
                           .Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value);
                if (roleList != null && roleList.Count() > 0)
                {
                    return roleList.ToList();
                }
                else
                {
                    return new List<string>();
                }

            }
            catch (Exception e)
            {

                return new List<string>();
            }
        }
    }
}
