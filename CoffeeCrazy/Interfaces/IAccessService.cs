﻿namespace CoffeeCrazy.Interfaces
{
    public interface IAccessService
    {
        string GetLoggedUserEmail(HttpContext httpContext);
        int? GetLoggedUserRoleId(HttpContext httpContext);
        string GetLoggedUserFirstName(HttpContext httpContext);
        int? GetLoggedUserId(HttpContext httpContext);


        bool IsUserLoggedIn(HttpContext httpContext);
        bool HasPermissionToDeleteAdmin(HttpContext httpContext, int userIdToDelete);
    }
}
