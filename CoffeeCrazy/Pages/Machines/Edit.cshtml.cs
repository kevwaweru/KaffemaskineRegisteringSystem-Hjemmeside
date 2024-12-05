using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class EditModel : PageModel
    {
        private readonly ICRUDRepo<Machine> _machineRepo;

        public EditModel(ICRUDRepo<Machine> machineRepo)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Machine.MachineId = id;
            // Opdaterer maskinen i databasen
            await _machineRepo.UpdateAsync(Machine);

            // Redirecter til Index (oversigten)
            return RedirectToPage("./Index");
        }
    }
}
