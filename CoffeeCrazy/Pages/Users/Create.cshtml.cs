using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User NewUser { get; set; }

        private IUserRepo _userRepo;

        public CreateModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
            NewUser = new User();
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userRepo.CreateAsync(NewUser);
            return RedirectToPage("Index");
        }
    }
}
