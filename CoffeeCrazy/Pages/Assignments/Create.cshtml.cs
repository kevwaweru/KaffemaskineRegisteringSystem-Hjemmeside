using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly ICRUDRepo<Assignment> _assignementRepo;
        public CreateModel(ICRUDRepo<Assignment> assignmentRepo)
        {
            _assignementRepo = assignmentRepo;
        }

        [BindProperty]
        public Assignment Assignment { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _assignementRepo.CreateAsync(Assignment);
            return RedirectToPage("./Index");
        }
    }
}
