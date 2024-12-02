using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class DeleteModel : PageModel
    {
        private readonly IJobTemplateRepo _assignmentSetRepo;

        public DeleteModel(IJobTemplateRepo assignmentSetRepo)
        {
            _assignmentSetRepo = assignmentSetRepo;
        }

        [BindProperty]
        public JobTemplate AssignmentSet { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            AssignmentSet = await _assignmentSetRepo.GetByIdAsync(id);
            if (AssignmentSet == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (AssignmentSet == null)
            {
                return NotFound();
            }

            await _assignmentSetRepo.DeleteAsync(AssignmentSet);
            return RedirectToPage("/Index");
        }
    }
}
