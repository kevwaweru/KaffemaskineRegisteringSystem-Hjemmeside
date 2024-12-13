using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly ICRUDRepo<User> _userRepo;
        private readonly IAccessService _accessService;
        public int Message { get; set; }

        public DeleteModel(ICRUDRepo<User> userRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
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

            await _userRepo.DeleteAsync(await _userRepo.GetByIdAsync(id));

            return RedirectToPage("/Users/Index");
        }
    }
}

