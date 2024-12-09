using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly IImageService _imageService;

        public DetailsModel(IUserRepo userRepo, IImageService userImage)
        {
            _userRepo = userRepo;
            _imageService = userImage;
        }

        public User User { get; set; }

        // Impl noget det gør så man ikke kan se andres details, men kun den der er logget ind.

        public IFormFile UserImage { get; set; }


        public async Task<IActionResult> OnGetAsync(int userId)
        {

            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToPage("/Login/Login");
            }


                User = await _userRepo.GetByIdAsync(userId);
                UserImage = _imageService.ConvertArrayToIFormFile(User.UserImage); //validering hvis null.

                if (User == null)
                {
                    return NotFound("Brugeren blev ikke fundet.");
                }

                return Page();
            
        }
    }
}
