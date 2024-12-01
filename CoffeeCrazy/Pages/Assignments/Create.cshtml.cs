using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly ITaskRepo _assignmentRepo;
        public CreateModel(ITaskRepo assignmentRepo)
        {
            _assignmentRepo = assignmentRepo;
        }

        [BindProperty]
        public Models.Task Assignment { get; set; }

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

            await _assignmentRepo.CreateAsync(Assignment);
            return RedirectToPage("/Index");
        }
    }
}
