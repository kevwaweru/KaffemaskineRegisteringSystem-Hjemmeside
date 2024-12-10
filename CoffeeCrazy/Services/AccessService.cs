using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Services
{
    public class AccessService : IAccessService
    {
        /// <summary>
        /// Retrieves the logged-in user's email from the session.
        /// </summary>
        /// <param name="httpContext">The current User HTTP context.</param>
        /// <returns>The logged-in user's email as a string, or null if not found.</returns>
        public string GetLoggedUserEmail(HttpContext httpContext)
        {
            return httpContext.Session.GetString("Email");
        }

        /// <summary>
        /// Retrieves the logged-in user's first name from the session.
        /// </summary>
        /// <param name="httpContext">The current User HTTP context.</param>
        /// <returns>The logged-in user's first name as a string, or null if not found.</returns>
        public string GetLoggedUserFirstName(HttpContext httpContext)
        {
            return httpContext.Session.GetString("FirstName");
        }

        /// <summary>
        /// Retrieves the logged-in user's role ID from the session.
        /// </summary>
        /// <param name="httpContext">The current User HTTP context.</param>
        /// <returns>The logged-in user's role ID as an integer, or null if not found.</returns>
        public int? GetLoggedUserRoleId(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("RoleId");
        }

        /// <summary>
        /// Retrieves the logged-in user's user ID from the session.
        /// </summary>
        /// <param name="httpContext">The current User HTTP context.</param>
        /// <returns>The logged-in user's user ID as an integer, or null if not found.</returns>
        public int? GetLoggedUserId(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("UserId");
        }

        /// <summary>
        /// Checks if the logged-in user has permission to delete another admin user.
        /// </summary>
        /// <param name="httpContext">The current User HTTP context containing session data.</param>
        /// <param name="userIdToDelete">The user ID of the admin to delete.</param>
        /// <returns>
        /// True if the logged-in user is not the same as the user to delete and has an admin role
        /// otherwise, false.
        /// </returns>
        public bool HasPermissionToDeleteAdmin(HttpContext httpContext, int userIdToDelete)
        {
            var loggedUserId = GetLoggedUserId(httpContext);
            var userRole = GetLoggedUserRoleId(httpContext);
            return loggedUserId != userIdToDelete && userRole == (int)Role.Admin;
        }

        /// <summary>
        /// Checks if a user is logged in by verifying if their user ID is present in the session.
        /// </summary>
        /// <param name="httpContext">The current HTTP User context containing session data.</param>
        /// <returns>True if a user ID is present in the session. otherwise false.</returns>
        public bool IsUserLoggedIn(HttpContext httpContext)
        {
            return GetLoggedUserId(httpContext).HasValue;
        }

        /// <summary>
        /// Checks if the logged-in user has an Admin role.
        /// </summary>
        /// <param name="httpContext">The current HTTP User context containing session data.</param>
        /// <returns>True if the logged-in user is an Admin, otherwise false.</returns>
        public bool IsAdmin(HttpContext httpContext)
        {
            return GetLoggedUserRoleId(httpContext) == (int)Role.Admin;
        }

        /// <summary>
        /// Checks if the logged-in user has a MasterAdmin (Super Admin) role.
        /// </summary>
        /// <param name="httpContext">The current HTTP User context containing session data.</param>
        /// <returns>True if the logged-in user is a MasterAdmin. otherwise false.</returns>
        public bool IsSuperAdmin(HttpContext httpContext)
        {
            return GetLoggedUserRoleId(httpContext) == (int)Role.MasterAdmin;
        }

        /// <summary>
        /// Checks if the logged-in user has an Employee role.
        /// </summary>
        /// <param name="httpContext">The current HTTP User context containing session data.</param>
        /// <returns>True if the logged-in user is an Employee. otherwise false.</returns>
        public bool IsEmployee(HttpContext httpContext)
        {
            return GetLoggedUserRoleId(httpContext) == (int)Role.Employee;
        }
    }
}
