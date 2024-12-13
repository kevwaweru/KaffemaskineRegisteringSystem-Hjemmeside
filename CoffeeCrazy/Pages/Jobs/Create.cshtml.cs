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
        private readonly IAccessService _accessService;

        public List<Machine> Machines { get; set; } = new ();

        [BindProperty]
        public Job NewJob { get; set; } = new Job();

        public CreateModel(ICRUDRepo<Job> jobRepo, ICRUDRepo<Machine> machineRepo, IAccessService accessService)
        {
            _jobRepo = jobRepo;
            _machineRepo = machineRepo;
            _accessService = accessService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }
            if (!_accessService.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Errors/AccessDenied");
            }

            Machines = await _machineRepo.GetAllAsync();
            return Page();
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
                    jobToBeCreated.FrequencyId = NewJob.FrequencyId;
                    jobToBeCreated.MachineId = machine.MachineId;
                    
                    await _jobRepo.CreateAsync(jobToBeCreated);
                }
            }
            else
            {
                await _jobRepo.CreateAsync(NewJob);
            }
            return RedirectToPage("/Jobs/Index");
        }

    }
}