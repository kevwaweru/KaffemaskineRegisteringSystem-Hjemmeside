using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class IndexModel : PageModel
    {
        private readonly IAssignmentSetRepo _assignmentSetRepo;

        public IndexModel(IAssignmentSetRepo assignmentSetRepo)
        {
            _assignmentSetRepo = assignmentSetRepo;
        }

        public List<AssignmentSet> AssignmentSets { get; set; }

        public async Task OnGetAsync()
        {
            AssignmentSets = await _assignmentSetRepo.GetAllAsync();
        }
    }


}
