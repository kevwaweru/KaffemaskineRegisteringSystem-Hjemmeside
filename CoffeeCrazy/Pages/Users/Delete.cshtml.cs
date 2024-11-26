using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Pages.Users
{
     public class DeleteModel : PageModel
        {
            private readonly IUserRepo _userRepo;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public DeleteModel(IUserRepo userRepo, IHttpContextAccessor httpContextAccessor)
            {
                _userRepo = userRepo;
                _httpContextAccessor = httpContextAccessor;
            }

            [BindProperty]
            public int UserIdToDelete { get; set; }

            public async Task<IActionResult> OnPostDeleteAsync()
            {
                User userToDelete = await _userRepo.GetByIdAsync(UserIdToDelete);

                try
                {
                    await _userRepo.DeleteAsync(userToDelete);
                }
                catch (UnauthorizedAccessException)
                {
                    ModelState.AddModelError(string.Empty, "Only admins are allowed to delete users.");
                    return Page();
                }
                catch (InvalidOperationException)
                {
                    ModelState.AddModelError(string.Empty, "You cannot delete your own account.");
                    return Page();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                    return Page();
                }

                return RedirectToPage("UserList");
            }
    } 
}

