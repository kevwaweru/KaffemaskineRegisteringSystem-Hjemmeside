using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class CreateModel : PageModel
    {
        private readonly IJobRepo _jobRepo;
        private readonly IJobTemplateRepo _jobTemplateRepo;
        private readonly IMachineRepo _machineRepo;


        public List<JobTemplate> TaskTemplates { get; set; } = new();
        public List<Machine> Machines { get; set; } = new();

        [BindProperty]
        public Job NewJob { get; set; } = new Job();

        public CreateModel(IJobRepo jobRepo, IJobTemplateRepo jobTemplateRepo, IMachineRepo machineRepo)
        {
            _jobRepo = jobRepo;
            _jobTemplateRepo = jobTemplateRepo;
            _machineRepo = machineRepo;
        }
        public async Task OnGetAsync()
        {
            // Hent alle templates
            TaskTemplates = await _jobTemplateRepo.GetAllAsync();
            Machines = await _machineRepo.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TaskTemplates = await _jobTemplateRepo.GetAllAsync(); // Reload templates if validation fails
                return Page();
            }

            NewJob.CreatedDate = DateTime.UtcNow;
            NewJob.Deadline = DateTime.UtcNow.AddDays(1);
            NewJob.IsCompleted = false;
            NewJob.Comment = "Pikhoved";

            await _jobRepo.CreateAsync(NewJob);

            return RedirectToPage("/Jobs/Index");
        }

    }
}