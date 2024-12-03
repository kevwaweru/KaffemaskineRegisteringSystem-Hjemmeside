using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class CreateModel : PageModel
    {
        private readonly IJobRepo _jobRepo;
        private readonly IJobTemplateRepo _jobTemplateRepo;
        private readonly IMachineRepo _machineRepo;

        public CreateModel(IJobRepo jobRepo, IJobTemplateRepo jobTemplateRepo, IMachineRepo machineRepo)
        {
            _jobRepo = jobRepo;
            _jobTemplateRepo = jobTemplateRepo;
            _machineRepo = machineRepo;
        }

        [BindProperty]
        public int JobTemplateId { get; set; }

        [BindProperty]
        public int MachineId { get; set; }
        public int comment { get; set; }

        [BindProperty]
        public int FrequencyId { get; set; }

        public List<JobTemplate> TaskTemplates { get; set; } = new();

        public List<Machine> Machines { get; set; } = new();

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
                TaskTemplates = await _jobTemplateRepo.GetAllAsync(); 
                return Page();
            }

            var newJob = new Job
            {
                JobTemplateId = JobTemplateId,
                MachineId = MachineId,
                CreatedDate = DateTime.UtcNow,
                Deadline = DateTime.UtcNow.AddDays(1),
                IsCompleted = false,
                FrequencyId = FrequencyId,
                Comment = "dillerdaller"

                //Comment And UserId Is missing, but it shoulden be a problem because they can be null. and is something that is getting set when job done.
            };
            

            await _jobRepo.CreateAsync(newJob);

            return RedirectToPage("/Jobs/Index");
        }
    }
}