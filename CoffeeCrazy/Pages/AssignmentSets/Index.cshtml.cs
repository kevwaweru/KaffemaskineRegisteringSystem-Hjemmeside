using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class IndexModel : PageModel
    {
        private readonly IJobTemplateRepo _assignmentSetRepo;

        public IndexModel(IJobTemplateRepo assignmentSetRepo)
        {
            _assignmentSetRepo = assignmentSetRepo;
        }

        public List<JobTemplate> AssignmentSets { get; set; }

        public async Task OnGetAsync()
        {
            AssignmentSets = await _assignmentSetRepo.GetAllAsync();
        }
    }


}
