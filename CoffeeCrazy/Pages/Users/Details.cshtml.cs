using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DetailsModel : PageModel
    {
        public User User { get; set; }

        private readonly IUserRepo _userRepo;
        private readonly IAccessService _accessService;

        public DetailsModel(IUserRepo userRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            if (!_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Login/Login");


            User = await _userRepo.GetByIdAsync(id);


            if (User == null)
            {
                return NotFound("Brugeren blev ikke fundet.");
            }

            return Page();

        }
    }
}
