using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class UserDetailModel : PageModel
    {
        private readonly IUserRepo _userRepo;

        public UserDetailModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToPage("/Login/Login");
            }

            try
            {
                User = await _userRepo.GetByIdAsync(userId);

                if (User == null)
                {
                    return NotFound("Brugeren blev ikke fundet.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Der opstod en fejl. Prøv igen senere.");
            }
        }
    }
}
