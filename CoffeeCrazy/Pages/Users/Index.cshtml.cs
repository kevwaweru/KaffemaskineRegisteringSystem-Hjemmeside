using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class IndexModel : PageModel
    {
        public List<User> Users { get; private set; }

        private readonly IUserRepo _UserRepo;
        public IndexModel(IUserRepo userCrudRepository)
        {
            _UserRepo = userCrudRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Users = await _UserRepo.GetAllAsync();

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
