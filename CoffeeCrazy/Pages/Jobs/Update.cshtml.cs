using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class UpdateModel : PageModel
    {
        private readonly IJobRepo _jobRepo;
        private readonly IMachineRepo _machineRepo;
        private readonly IJobTemplateRepo _jobTemplateRepo; 
        public UpdateModel(IJobRepo jobRepo, IMachineRepo machineRepo, IJobTemplateRepo jobTemplateRepo)
        {
            _machineRepo = machineRepo;
            _jobRepo = jobRepo;
           _jobTemplateRepo = jobTemplateRepo; 
        }
        public List<Machine> Machines { get; set; } = new();
        public List<JobTemplate> JobTemplates { get; set; } = new();

        [BindProperty]
        public Job JobToUpdate { get; set; }

        public async Task <IActionResult> OnGetAsync(int Id)
        {
            JobToUpdate = await _jobRepo.GetByIdAsync(Id);
            Machines = await _machineRepo.GetAllAsync();
            JobTemplates = await _jobTemplateRepo.GetAllAsync();

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
