using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class DeleteModel : PageModel
    {
        private readonly IMachineRepo _machineRepo;

        [BindProperty]
        public Machine Machine { get; set; }

        public DeleteModel(IMachineRepo machineRepo)
        {
            _machineRepo = machineRepo;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Machine = await _machineRepo.GetByIdAsync(id);
            if (Machine == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Machine == null)
            {
                return NotFound();
            }

            await _machineRepo.DeleteAsync(Machine);
            return RedirectToPage("./Index"); 
        }
    }
}
