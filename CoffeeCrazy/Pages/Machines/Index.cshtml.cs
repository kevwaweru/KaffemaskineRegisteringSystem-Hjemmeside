using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeCrazy.Pages.Machines
{
    public class IndexModel : PageModel
    {
        private readonly IMachineRepo _machineRepo;

        public IndexModel(IMachineRepo machineRepo)
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
