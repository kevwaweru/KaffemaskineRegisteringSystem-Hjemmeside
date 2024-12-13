using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace CoffeeCrazy.Pages.Machines
{
    public class DetailsModel : PageModel
    {
        private readonly ICRUDRepo<Machine> _machineRepo;
        private readonly ICRUDRepo<Job> _jobRepo;
        private readonly IAccessService _accessService;

        [BindProperty]
        public Machine Machine { get; set; }
        [BindProperty]
        public List<Job> Jobs { get; set; } = new List<Job>();

        public DetailsModel(ICRUDRepo<Machine> machineRepo, ICRUDRepo<Job> jobRepo, IAccessService accessService)
        {
            _machineRepo = machineRepo;
            _jobRepo = jobRepo;
            _accessService = accessService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }

            // Get the machine details based on ID
            Machine = await _machineRepo.GetByIdAsync(id);
            if (Machine == null)
            {
                return NotFound();
            }

            var allJobs = await _jobRepo.GetAllAsync();
            Jobs = allJobs
                .Where(job => job.MachineId == id && job.Deadline > DateTime.Now)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

            if (Jobs == null || !Jobs.Any())
            {
                return Page();
            }

            foreach (var job in Jobs)
            {
                var jobToUpdate = await _jobRepo.GetByIdAsync(job.JobId);
                if (jobToUpdate != null)
                {
                    if (jobToUpdate.IsCompleted != job.IsCompleted || jobToUpdate.Comment != job.Comment)
                    {
                        jobToUpdate.IsCompleted = job.IsCompleted;
                        jobToUpdate.Comment = job.Comment;
                        await _jobRepo.UpdateAsync(jobToUpdate);
                    }
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
