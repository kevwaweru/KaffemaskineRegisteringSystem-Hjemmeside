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
        private readonly IJobTemplateRepo _jobTemplateRepo;  // skal måske ikke bruges
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
        //[BindProperty]
        //public JobTemplate JobTemplate { get; set; }

        public async Task <IActionResult> OnGetAsync(int jobId, int jobTemplateId)
        {
            JobToUpdate = await _jobRepo.GetByIdAsync(jobId);
            Machines = await _machineRepo.GetAllAsync();
            JobTemplates = await _jobTemplateRepo.GetAllAsync();
            //      JobTemplate = await _jobTemplateRepo.GetByIdAsync(jobTemplateId);// skal måske ikke bruges

            if (JobToUpdate == null /* || JobTemplate == null*/)
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

      //    await _jobTemplateRepo.UpdateAsync(JobTemplate); // skal måske ikke bruges

            return RedirectToPage("/Jobs/Index");
        }
    }
}
