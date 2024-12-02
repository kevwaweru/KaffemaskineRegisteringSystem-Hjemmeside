using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly IJobRepo _assignmentRepo;
        public CreateModel(IJobRepo assignmentRepo)
        {
            _assignmentRepo = assignmentRepo;
        }

        [BindProperty]
        public Models.Job Assignment { get; set; }

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
