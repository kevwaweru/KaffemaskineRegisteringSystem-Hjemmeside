using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
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
        public JobTemplate AssignmentSet { get; set; } = new();
        public List<Models.Job> Assignments { get; set; } = new();
        [BindProperty]
        public List<int> SelectedAssignments { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            Assignments =  await _jobRepository.GetAllAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Assignments = await _jobRepository.GetAllAsync();
                return Page();
            }
            AssignmentSet.Deadline = DateTime.FromOADate(7);
            AssignmentSet.SetCompleted = false;
            await _jobTemplateRepo.CreateAsync(AssignmentSet);

            if (SelectedAssignments.Any())
            {
                await _AssignmentJunctionRepo.AddAssignmentToAssignmentSetAsync(AssignmentSet.AssignmentSetId,SelectedAssignments);
            }

            return RedirectToPage("/Index");
        }
    }
}
