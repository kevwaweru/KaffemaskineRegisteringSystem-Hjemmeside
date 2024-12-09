using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class CreateModel : PageModel
    {
        private IUserRepo _userRepo;
        private IImageService _imageService;
        public IFormFile PictureToBeUploaded { get; set; }

        [BindProperty]
        public User NewUser { get; set; } = new User();

        public CreateModel(IUserRepo userRepo, IImageService imageService)
        {
            _userRepo = userRepo;
            _imageService = imageService;
        }

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

            NewUser.UserImage = _imageService.ConvertImageToByteArray(PictureToBeUploaded);
            
            await _userRepo.CreateAsync(NewUser);
            return RedirectToPage("Index");
        }
    }
}
