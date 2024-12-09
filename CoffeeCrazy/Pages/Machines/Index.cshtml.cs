using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class IndexModel : PageModel
    {
        private readonly ICRUDRepo<Machine> _machineRepo;
        private readonly ICRUDRepo<Job> _jobRepo;

        public IndexModel(ICRUDRepo<Machine> machineRepo, ICRUDRepo<Job> jobRepo)
        {
            _machineRepo = machineRepo;
            _jobRepo = jobRepo;
        }

        public List<Machine> Machines { get; set; } = new();
        public List<Job> Jobs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? Id)
        {
            Jobs = await _jobRepo.GetAllAsync();
            var user = HttpContext.Session.GetInt32("UserId");
            if (user == null)
            {
               return RedirectToPage("/Login/Login");
           
            }
            // Henter alle maskiner fra databasen
            Machines = await _machineRepo.GetAllAsync();
            
            return Page();
        }


    }
}
