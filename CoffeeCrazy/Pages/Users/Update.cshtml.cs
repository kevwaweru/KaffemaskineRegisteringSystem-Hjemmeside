using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class UpdateModel : PageModel
    {
        private readonly ICRUDRepo<User> _userRepo;
        private readonly IAccessService _accessService;
        private readonly IImageService _imageService;

        [BindProperty]
        public User UserToBeUpdated { get; set; }
        public string? Base64StringUserImage { get; set; }

        public UpdateModel(ICRUDRepo<User> userRepo, IAccessService accessService, IImageService imageService)
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
            if (!_accessService.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Errors/AccessDenied");
            }

            UserToBeUpdated = await _userRepo.GetByIdAsync(id);
            Base64StringUserImage = _imageService.FormFileToBase64String(UserToBeUpdated.UserImageFile);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("UserToBeUpdated.PasswordSalt");
            ModelState.Remove("UserToBeUpdated.Password");
            ModelState.Remove("UserToBeUpdated.UserImage");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userRepo.UpdateAsync(UserToBeUpdated);
            return RedirectToPage("Index");
        }
       
    }
}
