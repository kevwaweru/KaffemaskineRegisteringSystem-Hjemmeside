using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class DetailsModel : PageModel
    {
        private readonly IAssignmentSetRepo _setRepo;
        private readonly IAssignmentRepo _assignmentRepo;
        private readonly IAssignmentJunctionRepo _assignmentJunctionRepo;

        public DetailsModel(IAssignmentSetRepo setRepo, IAssignmentRepo assignmentRepo, IAssignmentJunctionRepo assignmentJunctionRepo)
        {
            _setRepo = setRepo;
            _assignmentRepo = assignmentRepo;
            _assignmentJunctionRepo = assignmentJunctionRepo;
        }
        public AssignmentSet AssignmentSet { get; set; }
        public List<Assignment> Assignments { get; set; }

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
