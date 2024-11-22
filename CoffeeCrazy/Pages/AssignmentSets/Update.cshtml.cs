using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class UpdateModel : PageModel
    {
        private readonly IAssignmentSetRepo _assignmentSetRepo;

        public UpdateModel(IAssignmentSetRepo repo)
        {
            _assignmentSetRepo = repo;
        }

        [BindProperty]
        public AssignmentSet AssignmentSet { get; set; }

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
