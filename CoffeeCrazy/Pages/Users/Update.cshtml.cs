using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class UpdateModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly IAccessService _accessService;

        public UpdateModel(IUserRepo userRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
        }

        [BindProperty]
        public User UserToBeUpdated { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Login/Login");

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
