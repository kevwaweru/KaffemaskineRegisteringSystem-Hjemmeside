using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DetailsModel : PageModel
    {
        public User User { get; set; }

        private readonly IUserRepo _userRepo;

        public DetailsModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToPage("/Login/Login");
            }


            User = await _userRepo.GetByIdAsync(id);


            if (User == null)
            {
                return NotFound("Brugeren blev ikke fundet.");
            }

            return Page();

        }
    }
}
