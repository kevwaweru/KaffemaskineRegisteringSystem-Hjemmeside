using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class CreateModel : PageModel
    {
        private IUserRepo _userRepo;

        public CreateModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public User NewUser { get; set; } = new User();

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            //ModelState.Remove("PasswordSalt");
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            await _userRepo.CreateAsync(NewUser);
            return RedirectToPage("Index");
        }
    }
}
