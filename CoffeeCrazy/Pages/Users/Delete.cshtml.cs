using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly IAccessService _accessService;
        //private int currentAdminUserId;

        public DeleteModel(IUserRepo userRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
        }

        public int Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            if (!_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Login/Login");

            if (!_accessService.HasPermissionToDeleteAdmin(HttpContext, id))
                return RedirectToPage("/Error");

            await _userRepo.DeleteAsync(await _userRepo.GetByIdAsync(id));

            return RedirectToPage("/Users/Index");
        }
    }
}

