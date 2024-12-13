using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly ICRUDRepo<User> _userRepo;
        private readonly IAccessService _accessService;
        private readonly IImageService _imageService;

        [BindProperty]
        public User User { get; set; }

        public string? Base64StringUserImage { get; set; }

        public DetailsModel(ICRUDRepo<User> userRepo, IAccessService accessService, IImageService imageService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
            _imageService = imageService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");

            }

            User = await _userRepo.GetByIdAsync(id);
            Base64StringUserImage = _imageService.FormFileToBase64String(User.UserImageFile);

            return Page();

        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            ModelState.Remove("User.PasswordSalt");
            ModelState.Remove("User.Password");
            User existingUser = await _userRepo.GetByIdAsync(id);

            if (!ModelState.IsValid)
            {
                Base64StringUserImage = _imageService.FormFileToBase64String(existingUser.UserImageFile);
                return Page();
            }

            if (User.UserImageFile == null)
            {
                User.UserImageFile = existingUser.UserImageFile;
            }

            await _userRepo.UpdateAsync(User);

            Base64StringUserImage = _imageService.FormFileToBase64String(User.UserImageFile);

            return Page();
        }
    }
}
