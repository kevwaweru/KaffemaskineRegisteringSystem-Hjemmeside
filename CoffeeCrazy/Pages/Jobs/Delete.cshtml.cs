using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class DeleteModel : PageModel
    {
        private readonly ICRUDRepo<Job> _jobRepo;
        private readonly IAccessService _accessService;

        public DeleteModel(ICRUDRepo<Job> jobRepo, IAccessService accessService)
        {
            _jobRepo = jobRepo;
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

            await _jobRepo.DeleteAsync(await _jobRepo.GetByIdAsync(id));

            return RedirectToPage("Index");
        }
    }
}
