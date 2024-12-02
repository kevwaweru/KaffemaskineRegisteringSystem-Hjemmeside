using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class DetailsModel : PageModel
    {
        private readonly IJobTemplateRepo _setRepo;
        private readonly IJobRepo _assignmentRepo;
        private readonly IAssignmentJunctionRepo _assignmentJunctionRepo;

        public DetailsModel(IJobTemplateRepo setRepo, IJobRepo assignmentRepo, IAssignmentJunctionRepo assignmentJunctionRepo)
        {
            _setRepo = setRepo;
            _assignmentRepo = assignmentRepo;
            _assignmentJunctionRepo = assignmentJunctionRepo;
        }
        public JobTemplate AssignmentSet { get; set; }
        public List<Models.Job> Assignments { get; set; }

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
