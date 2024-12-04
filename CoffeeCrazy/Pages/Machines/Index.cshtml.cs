using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class IndexModel : PageModel
    {
        private readonly ICRUDRepo<Machine> _machineRepo;

        public IndexModel(ICRUDRepo<Machine> machineRepo)
        {
            _machineRepo = machineRepo;
        }

        public List<Machine> Machines { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Henter alle maskiner fra databasen
            Machines = await _machineRepo.GetAllAsync();
            return Page();
        }
    }
}
