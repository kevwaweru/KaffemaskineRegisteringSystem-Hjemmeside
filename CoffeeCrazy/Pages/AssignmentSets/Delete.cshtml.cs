using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class DeleteModel : PageModel
    {
        private readonly IAssignmentSetRepo _AssignemntSetrepo;

        public DeleteModel(IAssignmentSetRepo repo)
        {
            _AssignemntSetrepo = repo;
        }

        [BindProperty]
        public AssignmentSet AssignmentSet { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            AssignmentSet = await _AssignemntSetrepo.GetByIdAsync(id);

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

            await _AssignemntSetrepo.DeleteAsync(AssignmentSet);
            return RedirectToPage("Index");
        }
    }
}
