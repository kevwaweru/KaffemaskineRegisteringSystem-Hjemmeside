using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class UpdateModel : PageModel
    {
        private readonly IJobRepo _jobRepo;

        public UpdateModel(IJobRepo repo)
        {
            _jobRepo = repo;
        }

        [BindProperty]
        public Job JobToUpdate { get; set; } = new Job();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                // Hent det eksisterende job
                JobToUpdate = await _jobRepo.GetByIdAsync(id);

                if (JobToUpdate == null)
                {
                    return NotFound(); // Returnér 404, hvis job ikke findes
                }

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Fejl under indlæsning: {ex.Message}");
                return RedirectToPage("/Jobs/Index"); // Gå til en oversigtsside ved fejl
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _jobRepo.UpdateAsync(JobToUpdate);
                return RedirectToPage("/Jobs/Index"); // Opdater og gå til oversigt
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Fejl under opdatering: {ex.Message}");
                return Page();
            }
        }
    }
}
