using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.AccessControl;

namespace CoffeeCrazy.Pages.Jobz
{
    public class IndexModel : PageModel
    {
        private readonly IJobTemplateRepo _jobTemplateRepo;
        private readonly IJobRepo _jobRepo;

        public IndexModel(IJobTemplateRepo jobTemplateRepo, IJobRepo jobRepo)
        {
            _jobTemplateRepo = jobTemplateRepo;
            _jobRepo = jobRepo;
        }

        public List<JobTemplate> JobTemplates { get; set; }
        public List<Job> Jobs { get; set; }

        public async Task OnGetAsync()
        {
            JobTemplates = await _jobTemplateRepo.GetAllAsync();
            Jobs = await _jobRepo.GetAllAsync();
        }
    }


}
