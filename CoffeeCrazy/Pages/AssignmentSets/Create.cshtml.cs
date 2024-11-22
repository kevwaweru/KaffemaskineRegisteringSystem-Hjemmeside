using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.AssignmentSets
{
    public class CreateModel : PageModel
    {
        private readonly IAssignmentSetRepo _AssignmentSetRepo;

        public CreateModel(IAssignmentSetRepo AssignmentSetRepo)
        {
            _AssignmentSetRepo = AssignmentSetRepo;
        }

        [BindProperty]
        public AssignmentSet AssignmentSetToUpload { get; set; }

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
            AssignmentSetToUpload.Deadline = DateTime.FromOADate(7);
            AssignmentSetToUpload.SetCompleted = false;

            await _AssignmentSetRepo.CreateAsync(AssignmentSetToUpload);
            return RedirectToPage("Index");
        }
    }
}
