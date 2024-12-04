using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class CreateModel : PageModel
    {
        private readonly ICRUDRepo<Job> _jobRepo;
        private readonly ICRUDRepo<Machine> _machineRepo;

        public List<Machine> Machines { get; set; } = new ();

        [BindProperty]
        public Job NewJob { get; set; } = new Job();

        public CreateModel(ICRUDRepo<Job> jobRepo, ICRUDRepo<Machine> machineRepo)
        {
            _jobRepo = jobRepo;
            _machineRepo = machineRepo;
        }
        public async Task OnGetAsync()
        {
            Machines = await _machineRepo.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (NewJob.MachineId == 99)
            {
                foreach (Machine machine in await _machineRepo.GetAllAsync())
                {
                    Job jobToBeCreated = new Job();

                    jobToBeCreated.Title = NewJob.Title;
                    jobToBeCreated.Description = NewJob.Description;
                    jobToBeCreated.Comment = "null";
                    jobToBeCreated.IsCompleted = false;
                    jobToBeCreated.DateCreated = DateTime.UtcNow;
                    jobToBeCreated.Deadline = DateTime.UtcNow.AddMinutes(1);
                    jobToBeCreated.FrequencyId = NewJob.FrequencyId;
                    jobToBeCreated.MachineId = machine.MachineId;
                    
                    await _jobRepo.CreateAsync(jobToBeCreated);
                }
            }
            else
            {
                NewJob.IsCompleted = false;
                NewJob.Comment = "null";
                NewJob.DateCreated = DateTime.UtcNow;
                NewJob.Deadline = DateTime.UtcNow.AddMinutes(1);
                await _jobRepo.CreateAsync(NewJob);
            }
            return RedirectToPage("/Jobs/Index");
        }

    }
}