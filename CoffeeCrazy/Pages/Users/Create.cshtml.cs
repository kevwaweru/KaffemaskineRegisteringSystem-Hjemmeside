using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly IAccessService _accessService;
        private IImageService _imageService;

        [BindProperty]
        public User NewUser { get; set; } = new User();

        public CreateModel(IUserRepo userRepo, IImageService imageService, IAccessService accessService)
        {
            _userRepo = userRepo;
            _imageService = imageService;
            _accessService = accessService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }

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
