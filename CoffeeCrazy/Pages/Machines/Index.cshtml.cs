using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class IndexModel : PageModel
    {
        private readonly ICRUDRepo<Machine> _machineRepo;
        private readonly ICRUDRepo<Job> _jobRepo;
        private readonly IAccessService _accessService;

        public IndexModel(ICRUDRepo<Machine> machineRepo, ICRUDRepo<Job> jobRepo, IAccessService accessService)
        {
            _machineRepo = machineRepo;
            _jobRepo = jobRepo;
            _accessService = accessService;
        }
        
        public List<Machine> Machines { get; set; } = new();
        public List<Job> Jobs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? Id)
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }

            Jobs = await _jobRepo.GetAllAsync();
          
            
            // Henter alle maskiner fra databasen
            Machines = await _machineRepo.GetAllAsync();
            
            return Page();
        }


    }
}
