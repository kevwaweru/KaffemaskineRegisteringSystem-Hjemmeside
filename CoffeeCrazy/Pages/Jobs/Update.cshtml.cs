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

        public List<Machine> Machines { get; set; } = new();

        [BindProperty]
        public Job JobToUpdate { get; set; }

        public UpdateModel(ICRUDRepo<Job> jobRepo, ICRUDRepo<Machine> machineRepo)
        {
            _jobRepo = jobRepo;
            _machineRepo = machineRepo;
        }

        public async Task <IActionResult> OnGetAsync(int Id)
        {
            JobToUpdate = await _jobRepo.GetByIdAsync(Id);
            Machines = await _machineRepo.GetAllAsync();

            if (JobToUpdate == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
 
            await _jobRepo.UpdateAsync(JobToUpdate);

            return RedirectToPage("/Jobs/Index");
        }
    }
}
