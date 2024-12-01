using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class IndexModel : PageModel
    {
        private readonly ITaskTemplateRepo _assignmentSetRepo;

        public IndexModel(ITaskTemplateRepo assignmentSetRepo)
        {
            _assignmentSetRepo = assignmentSetRepo;
        }

        public List<TaskTemplate> AssignmentSets { get; set; }

        public async Task OnGetAsync()
        {
            AssignmentSets = await _assignmentSetRepo.GetAllAsync();
        }
    }


}
