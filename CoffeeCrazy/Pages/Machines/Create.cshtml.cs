using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Machines
{
    public class CreateModel : PageModel
    {
        private readonly ICRUDRepo<Machine> _machineRepo;
        private IImageService _imageService;
        private readonly IAccessService _accessService;

        public IFormFile PictureToBeUploaded { get; set; }

        [BindProperty]
        public Machine Machine { get; set; }

        public CreateModel(ICRUDRepo<Machine> machineRepo, IImageService imageService, IAccessService accessService)
        {
            _machineRepo = machineRepo;
            _imageService = imageService;
            _accessService = accessService;
        }

        public IActionResult OnGet()
        {
            if (!_accessService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login/Login");
            }
            if (!_accessService.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Errors/AccessDenied");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Machine.MachineImage = _imageService.FormFileToByteArray(PictureToBeUploaded);

            await _machineRepo.CreateAsync(Machine);
            return RedirectToPage("./Index"); // Redirect to a list page
        }
    }
}