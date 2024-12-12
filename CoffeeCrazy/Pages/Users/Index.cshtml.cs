using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly IAccessService _accessService;
        private readonly IImageService _imageService;

        public List<User> Users { get; private set; } = new List<User>();
        public List<User> FilteredUsersByCampus { get; private set; } = new List<User>();
        public Dictionary<int, string?> UserImageBase64Strings { get; private set; } = new();

        [BindProperty(SupportsGet = true)]
        public Campus? CampusFilter { get; set; }

        public IndexModel(IUserRepo userRepo, IAccessService accessService, IImageService imageService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
            _imageService = imageService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }
            if (!_accessService.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Errors/AccessDenied");
            }

            try
            {
                Users = await _userRepo.GetAllAsync();

                if (!CampusFilter.HasValue)
                {
                    CampusFilter = Campus.Roskilde;
                }

                FilteredUsersByCampus = Users.Where(user => user.Campus == CampusFilter).ToList();

                foreach (User user in FilteredUsersByCampus)
                {
                    UserImageBase64Strings.Add(user.UserId, _imageService.FormFileToBase64String(user.UserImageFile));
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled error: {ex.Message}");
                return RedirectToPage("/Error", new { message = "Noget gik galt under hentning af brugere. Kontakt administrator." });
            }
        }
    }
}
