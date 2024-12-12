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
        public Job Job { get; set; }

        public List<Job> job { get; set; } = new List<Job>();

        public DetailsModel(ICRUDRepo<Machine> machineRepo, ICRUDRepo<Job> jobRepo, IAccessService accessService)
        {
            _machineRepo = machineRepo;
            _jobRepo = jobRepo;
            _accessService = accessService;
        }

        // Method to fetch machine and jobs from the database
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

            job = await _jobRepo.GetAllAsync();
            job = job.Where(job => job.MachineId == id && job.Deadline > DateTime.Now).ToList(); 

            return Page();
        }

        // Method to handle form submission for updating job status and comments
        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Update each job's IsCompleted and Comment from the form data
            foreach (var job in job)
            {
                var jobToUpdate = await _jobRepo.GetByIdAsync(job.JobId);
                if (jobToUpdate != null)
                {
                    jobToUpdate.IsCompleted = job.IsCompleted;
                    jobToUpdate.Comment = job.Comment;

                    // Update the job in the database
                    await _jobRepo.UpdateAsync(jobToUpdate);
                }
            }

            return RedirectToPage("./Details", new { id = id });
        }
    }
}
