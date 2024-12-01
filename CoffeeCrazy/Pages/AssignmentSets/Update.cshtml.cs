using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class UpdateModel : PageModel
    {
        private readonly ITaskTemplateRepo _assignmentSetRepo;

        public UpdateModel(ITaskTemplateRepo repo)
        {
            _assignmentSetRepo = repo;
        }

        [BindProperty]
        public TaskTemplate AssignmentSet { get; set; }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _assignmentSetRepo.UpdateAsync(AssignmentSet);
            return RedirectToPage("Index");
        }
    }
}
