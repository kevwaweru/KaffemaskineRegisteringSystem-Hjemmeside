using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CoffeeCrazy.Pages.Machines
{
    public class EditModel : PageModel
    {
        private readonly IMachineRepo _machineRepo;

        public EditModel(IMachineRepo machineRepo)
        {
            _machineRepo = machineRepo;
        }

        [BindProperty]
        public Machine Machine { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Henter maskinen baseret på ID
            Machine = await _machineRepo.GetByIdAsync(id);
            if (Machine == null)
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

            // Opdaterer maskinen i databasen
            await _machineRepo.UpdateAsync(Machine);

            // Redirecter til Index (oversigten)
            return RedirectToPage("./Index");
        }
    }
}
