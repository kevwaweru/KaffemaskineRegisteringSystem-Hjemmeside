using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class EditModel : PageModel
    {
        private readonly ICRUDRepo<Machine> _machineRepo;
        private readonly IImageService _imageService;
        private readonly IAccessService _accessService;

        [BindProperty]
        public Machine MachinetoUpdate { get; set; }
        public string? Base64StringMachineImage { get; set; }

        public EditModel(ICRUDRepo<Machine> machineRepo, IImageService imageService, IAccessService accessService)
        {
            _machineRepo = machineRepo;
            _imageService = imageService;
            _accessService = accessService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }
            if (!_accessService.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Errors/AccessDenied");
            }


            // Henter maskinen baseret på ID
            MachinetoUpdate = await _machineRepo.GetByIdAsync(id);
            Base64StringMachineImage = _imageService.FormFileToBase64String(MachinetoUpdate.MachineImage);

            if (MachinetoUpdate == null)
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
           
            MachinetoUpdate.MachineId = id;
            // Opdaterer maskinen i databasen
            await _machineRepo.UpdateAsync(MachinetoUpdate);

            // Redirecter til Index (oversigten)
            return RedirectToPage("./Index");
        }
    }
}
