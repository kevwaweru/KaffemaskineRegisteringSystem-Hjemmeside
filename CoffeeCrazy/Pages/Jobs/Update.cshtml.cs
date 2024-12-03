using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class UpdateModel : PageModel
    {
        private readonly IJobRepo _jobRepo;
        // private readonly IJobTemplateRepo _jobTemplateRepo;  // skal måske ikke bruges
        public UpdateModel(IJobRepo jobRepo/*, IJobTemplateRepo jobTemplateRepo*/)
        {
            _jobRepo = jobRepo;
            //    _jobTemplateRepo = jobTemplateRepo; // skal måske ikke bruges
        }

        [BindProperty]
        public Job Job { get; set; }
        //[BindProperty]
        //public JobTemplate JobTemplate { get; set; }

        public async Task <IActionResult> OnGetAsync(int jobId, int jobTemplateId)
        {
            Job = await _jobRepo.GetByIdAsync(jobId);
      //      JobTemplate = await _jobTemplateRepo.GetByIdAsync(jobTemplateId);// skal måske ikke bruges

            if (Job == null /* || JobTemplate == null*/)
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

            await _jobRepo.UpdateAsync(Job);
      //      await _jobTemplateRepo.UpdateAsync(JobTemplate); // skal måske ikke bruges

            return RedirectToPage("/Jobs/Index");
        }
    }
}
