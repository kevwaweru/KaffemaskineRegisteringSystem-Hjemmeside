using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class CreateModel : PageModel
    {
        private readonly IMachineRepo _machineRepo;

        [BindProperty]
        public Machine Machine { get; set; }

        public CreateModel(IMachineRepo machineRepo)
        {
            _machineRepo = machineRepo;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _machineRepo.CreateAsync(Machine);
            return RedirectToPage("./Index"); // Redirect to a list page
        }
    }
}