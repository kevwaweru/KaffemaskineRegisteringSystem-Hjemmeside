using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class UpdateModel : PageModel
    {
        private readonly ICRUDRepo<Job> _jobRepo;
        private readonly ICRUDRepo<Machine> _machineRepo;
        private readonly IAccessService _accessService;
        public List<Machine> Machines { get; set; } = new();

        [BindProperty]
        public Job JobToUpdate { get; set; }

        public UpdateModel(ICRUDRepo<Job> jobRepo, ICRUDRepo<Machine> machineRepo, IAccessService accessService)
        {
            _jobRepo = jobRepo;
            _machineRepo = machineRepo;
            _accessService = accessService;
        }

        public async Task<IActionResult> OnGetAsync(int Id)
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }
            if (!_accessService.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Errors/AccessDenied");
            }

            JobToUpdate = await _jobRepo.GetByIdAsync(Id);
            Machines = await _machineRepo.GetAllAsync();

            if (JobToUpdate == null)
            {
                return NotFound();
            }
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            JobToUpdate.JobId = Id;
            await _jobRepo.UpdateAsync(JobToUpdate);

            return RedirectToPage("/Jobs/Index");
        }
    }
}
