using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class DeleteModel : PageModel
    {
        private readonly IJobRepo _jobRepo;

        public DeleteModel(IJobRepo jobRepo)
        {
            _jobRepo = jobRepo;

        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            await _jobRepo.DeleteAsync(await _jobRepo.GetByIdAsync(id));
            return RedirectToPage("Index");
        }
    }
}
