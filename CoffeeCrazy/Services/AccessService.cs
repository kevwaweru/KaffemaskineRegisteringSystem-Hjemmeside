using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Services
{
    public class AccessService : IAccessService
    {
        public string GetLoggedUserEmail(HttpContext httpContext)
        {
            return httpContext.Session.GetString("Email");
        }

        public string GetLoggedUserFirstName(HttpContext httpContext)
        {
            return httpContext.Session.GetString("FirstName");
        }

        public int? GetLoggedUserRoleId(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("RoleId");
        }

        public int? GetLoggedUserId(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("UserId");
        }
        /// <summary>
        /// Checks if the logged User and the IdToDelete is the same, and if the Role is admin.
        /// </summary>
        /// <param name="httpContext">Finds the logged user</param>
        /// <param name="userIdToDelete"></param>
        /// <returns></returns>
        public bool HasPermissionToDeleteAdmin(HttpContext httpContext, int userIdToDelete)
        {
            var loggedUserId = GetLoggedUserId(httpContext);
            var userRole = GetLoggedUserRoleId(httpContext);
            return loggedUserId != userIdToDelete && userRole == (int)Role.Admin;
        }
        /// <summary>
        /// Checks if httpContext has value
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>True if it has and false if dont</returns>
        public bool IsUserLoggedIn(HttpContext httpContext)
        {
            return GetLoggedUserId(httpContext).HasValue;
        }
        public bool IsAdmin(HttpContext httpContext)
        {
            return GetLoggedUserRoleId(httpContext) == (int)Role.Admin;
        }

        public bool IsSuperAdmin(HttpContext httpContext)
        {
            return GetLoggedUserRoleId(httpContext) == (int)Role.MasterAdmin;
        }

        public bool IsUser(HttpContext httpContext)
        {
            return GetLoggedUserRoleId(httpContext) == (int)Role.Employee;
        }
    }
}
