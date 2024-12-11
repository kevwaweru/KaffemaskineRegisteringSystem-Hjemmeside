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

        public List<User> Users { get; private set; } = new List<User>();
        public List<User> FilteredUsers { get; private set; } = new List<User>();

        [BindProperty(SupportsGet = true)]
        public Campus? CampusFilter { get; set; }

        public IndexModel(IUserRepo userRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }

            try
            {
                // Hent brugere
                Users = await _userRepo.GetAllAsync();

                // Standard til Campus Roskilde, hvis CampusFilter er null
                if (!CampusFilter.HasValue)
                {
                    CampusFilter = Campus.Roskilde;
                }

                // Filtrer brugere baseret på valgt Campus
                FilteredUsers = Users.Where(user => user.Campus == CampusFilter).ToList();

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
