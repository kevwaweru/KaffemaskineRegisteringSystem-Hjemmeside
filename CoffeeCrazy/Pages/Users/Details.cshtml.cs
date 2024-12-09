using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly IAccessService _accessService;

        public DetailsModel(IUserRepo userRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
        }

        public User User { get; set; }

        // Impl noget det gør så man ikke kan se andres details, men kun den der er logget ind.


        public async Task<IActionResult> OnGetAsync(int userId)
        {

            if (!_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Login/Login");

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
