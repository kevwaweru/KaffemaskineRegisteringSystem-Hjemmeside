using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.JobsTemplates
{
    public class CreateModel : PageModel
    {
        private readonly IJobTemplateRepo _jobTemplateRepo;
        public CreateModel(IJobTemplateRepo jobRepo)
        {
            _jobTemplateRepo = jobRepo;
        }

        [BindProperty]
        public JobTemplate JobTemplate { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _jobTemplateRepo.CreateAsync(JobTemplate);
            return RedirectToPage("/Index");
        }
    }
}
