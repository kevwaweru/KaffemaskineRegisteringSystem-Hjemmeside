using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class IndexModel : PageModel
    {
        private readonly IJobTemplateRepo _jobTemplateRepo;
        private readonly IJobRepo _jobRepo;
        private readonly IUserRepo _userRepo;

        public List<Frequency> frequencies = new List<Frequency>();

        public List<JobTemplate> JobTemplates { get; set; }
        public List<Job> Jobs { get; set; }
        public List<User> Users { get; set; }

        public IndexModel(IJobTemplateRepo jobTemplateRepo, IJobRepo jobRepo, IUserRepo userRepo)
        {
            _jobTemplateRepo = jobTemplateRepo;
            _jobRepo = jobRepo;
            _userRepo = userRepo;
        }

        public async Task OnGetAsync()
        {
            JobTemplates = await _jobTemplateRepo.GetAllAsync();
            Jobs = await _jobRepo.GetAllAsync();
            Users = await _userRepo.GetAllAsync();
            foreach (var item in Jobs)
            {
                frequencies.Add((Frequency)item.FrequencyId);
            }
        }
    }
}

