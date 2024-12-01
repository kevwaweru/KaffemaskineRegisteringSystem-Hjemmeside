using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class DetailsModel : PageModel
    {
        private readonly ITaskTemplateRepo _setRepo;
        private readonly ITaskRepo _assignmentRepo;
        private readonly IAssignmentJunctionRepo _assignmentJunctionRepo;

        public DetailsModel(ITaskTemplateRepo setRepo, ITaskRepo assignmentRepo, IAssignmentJunctionRepo assignmentJunctionRepo)
        {
            _setRepo = setRepo;
            _assignmentRepo = assignmentRepo;
            _assignmentJunctionRepo = assignmentJunctionRepo;
        }
        public TaskTemplate AssignmentSet { get; set; }
        public List<Models.Task> Assignments { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            AssignmentSet = await _setRepo.GetByIdAsync(id);

            if (AssignmentSet == null)
            {
                return NotFound();
            }

            Assignments = await _assignmentJunctionRepo.GetAllObjectsFromAssignmentJunctionsAsync(id);
            return Page();
        }
    }


}
