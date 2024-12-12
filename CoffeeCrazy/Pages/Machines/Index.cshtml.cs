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
        private readonly IImageService _imageService;

        public IndexModel(ICRUDRepo<Machine> machineRepo, ICRUDRepo<Job> jobRepo, IImageService imageService)
        {
            _machineRepo = machineRepo;
            _jobRepo = jobRepo;
            _imageService = imageService;
        }
        
        public List<Machine> Machines { get; set; } = new();
        public List<Job> Jobs { get; set; } = new();
        public Dictionary<int, string?> MachineImageBase64Strings { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync(int? Id)
        {
            Jobs = await _jobRepo.GetAllAsync();
            var user = HttpContext.Session.GetInt32("UserId");
            if (user == null)
            {
               return RedirectToPage("/Login/login");
           
            }

            // Henter alle maskiner fra databasen
            Machines = await _machineRepo.GetAllAsync();

            foreach (Machine machine in Machines)
            {
                MachineImageBase64Strings.Add(machine.MachineId, _imageService.FormFileToBase64String(machine.MachineImage));
            }

            return Page();
        }


    }
}
