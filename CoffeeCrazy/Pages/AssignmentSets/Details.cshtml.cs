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

        public DetailsModel(IAssignmentSetRepo setRepo, IAssignmentRepo assignmentRepo)
        {
            _setRepo = setRepo;
            _assignmentRepo = assignmentRepo;
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

            Assignments = await _setRepo.GetByAssignmentSetIdAsync(id);
            return Page();
        }
    }


}
