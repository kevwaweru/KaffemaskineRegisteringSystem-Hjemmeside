using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class IndexModel : PageModel
    {
        private readonly IJobTemplateRepo _jobTemplateRepo;
        private readonly IJobRepo _jobRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMachineRepo _machineRepo;


        public List<Frequency> frequencies = new List<Frequency>();

        public List<JobTemplate> JobTemplates { get; set; }
        public List<Job> Jobs { get; set; }
        public List<User> Users { get; set; }
        public List<Machine> Machines { get; set; }

        private List<Job> OlderThan6MonthsJobs { get; set; }

        public IndexModel(IJobTemplateRepo jobTemplateRepo, IJobRepo jobRepo, IUserRepo userRepo, IMachineRepo machineRepo)
        {
            _jobTemplateRepo = jobTemplateRepo;
            _jobRepo = jobRepo;
            _userRepo = userRepo;
            _machineRepo = machineRepo;

        }

        public async Task OnGetAsync()
        {

            //Finder jobs som er ældre end 6 måneder
            OlderThan6MonthsJobs = _jobRepo.GetAllAsync().Result.FindAll(parameter => parameter.Deadline < DateTime.UtcNow.AddMonths(-6)); 

            //Sletter jobs som er ældre end 6 måneder
            foreach (Job oldJob in OlderThan6MonthsJobs)
            {
                await _jobRepo.DeleteAsync(oldJob);
            }

            JobTemplates = await _jobTemplateRepo.GetAllAsync();
            Jobs = await _jobRepo.GetAllAsync();
            Users = await _userRepo.GetAllAsync();
            Machines = await _machineRepo.GetAllAsync();
            foreach (var item in Jobs)
            {
                frequencies.Add((Frequency)item.FrequencyId);
            }
        }
    }
}

