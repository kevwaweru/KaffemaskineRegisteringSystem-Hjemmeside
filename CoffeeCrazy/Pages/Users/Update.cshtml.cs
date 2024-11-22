using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class UpdateModel : PageModel
    {
        private readonly IUserRepo _userRepo;

        [BindProperty]
        public User UserToBeUpdated { get; set; }

        public UpdateModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult OnGet(int id)
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _userRepo.UpdateAsync(UserToBeUpdated);
            return RedirectToPage("ResidentOverview");
        }
    }
}
