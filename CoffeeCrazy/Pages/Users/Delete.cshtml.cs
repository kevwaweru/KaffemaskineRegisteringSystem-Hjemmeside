using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private int currentAdminUserId;

        [BindProperty]
        public int UserIdToDelete { get; set; }

        public DeleteModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;

        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            User userToDelete = await _userRepo.GetByIdAsync(UserIdToDelete);

            // Check if the user is trying to delete their own account
            if (userToDelete.UserId == currentAdminUserId)
            {
                ModelState.AddModelError(string.Empty, "You cannot delete your own account.");
                return Page();
            }

            // Check if the current user is an admin
            else if (userToDelete.Role != Role.Admin)
            {
                try
                {
                    // Try to delete the user
                    await _userRepo.DeleteAsync(userToDelete);
                }
                catch (Exception ex)
                {
                    // Catch any error that occurs during deletion
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                    return Page();
                }
            }
            // If the user is an admin, don't allow deletion
            else
            {
                ModelState.AddModelError(string.Empty, "Only admins are allowed to delete users.");
                return Page();
            }

            // Redirect to the User List page if deletion was successful
            return RedirectToPage("UserList");
        }
    }
}

