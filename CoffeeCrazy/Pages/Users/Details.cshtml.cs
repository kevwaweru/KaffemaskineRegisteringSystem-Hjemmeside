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
        private readonly IImageService _imageService;

        public User User { get; set; }
        public string? Base64StringUserImage { get; set; }

        public DetailsModel(IUserRepo userRepo, IAccessService accessService, IImageService imageService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
            _imageService = imageService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            if (!_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Login/Login");


            User = await _userRepo.GetByIdAsync(id);
            Base64StringUserImage = _imageService.FormFileToBase64String(User.UserImageFile);


            if (User == null)
            {
                return NotFound("Brugeren blev ikke fundet.");
            }

            return Page();

        }
    }
}
