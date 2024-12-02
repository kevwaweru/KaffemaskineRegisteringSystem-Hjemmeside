using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class CreateModel : PageModel
    {
        private readonly IJobTemplateRepo _jobTemplateRepo;
        private readonly IJobRepo _jobRepository;

        public CreateModel(IJobTemplateRepo AssignmentSetRepo, IJobRepo assignmentRepo)
        {
            _jobTemplateRepo = AssignmentSetRepo;
            _jobRepository = assignmentRepo;
        }

        [BindProperty]
        public Job Job { get; set; } = new();
        public List<JobTemplate> JobTemplates { get; set; } = new();
        [BindProperty]
        public List<int> SelectedJobTemplate { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            JobTemplates =  await _jobTemplateRepo.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                JobTemplates = await _jobTemplateRepo.GetAllAsync();
                return Page();
            }
            Job.Deadline = DateTime.FromOADate(7);
            Job.IsCompleted = false;
            await _jobRepository.CreateAsync(Job); 

            return RedirectToPage("/Index");
        }
    }
}
