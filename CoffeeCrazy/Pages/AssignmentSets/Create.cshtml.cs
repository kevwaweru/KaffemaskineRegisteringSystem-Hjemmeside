using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class CreateModel : PageModel
    {
        private readonly IAssignmentSetRepo _AssignmentSetRepo;
        private readonly IAssignmentJunctionRepo _AssignmentJunctionRepo;
        private readonly IAssignmentRepo _AssignmentRepo;

        public CreateModel(IAssignmentSetRepo AssignmentSetRepo, IAssignmentJunctionRepo assignmentJunctionRepo, IAssignmentRepo assignmentRepo)
        {
            _AssignmentSetRepo = AssignmentSetRepo;
            _AssignmentJunctionRepo = assignmentJunctionRepo;
            _AssignmentRepo = assignmentRepo;
        }

        [BindProperty]
        public AssignmentSet AssignmentSet { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
        [BindProperty]
        public List<int> SelectedAssignments { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            Assignments =  await _AssignmentRepo.GetAllAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Assignments = await _AssignmentRepo.GetAllAsync();
                return Page();
            }
            AssignmentSet.Deadline = DateTime.FromOADate(7);
            AssignmentSet.SetCompleted = false;
            await _AssignmentSetRepo.CreateAsync(AssignmentSet);

            if (SelectedAssignments.Any())
            {
                await _AssignmentJunctionRepo.AddAssignmentToAssignmentSetAsync(AssignmentSet.AssignmentSetId,SelectedAssignments);
            }

            return RedirectToPage("/Index");
        }
    }
}
