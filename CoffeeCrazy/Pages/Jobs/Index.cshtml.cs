using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Jobs
{
    public class IndexModel : PageModel
    {
        private readonly ICRUDRepo<Job> _jobRepo;
        private readonly IUserRepo _userRepo;
        private readonly ICRUDRepo<Machine> _machineRepo;


        public List<Frequency> Frequencies = new List<Frequency>();

        public List<Job> Jobs { get; set; }
        public List<User> Users { get; set; }
        public List<Machine> Machines { get; set; }

        private List<Job> OlderThan6MonthsJobs { get; set; }

        public IndexModel(ICRUDRepo<Job> jobRepo, IUserRepo userRepo, ICRUDRepo<Machine> machineRepo)
        {
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

            Jobs = await _jobRepo.GetAllAsync();
            Users = await _userRepo.GetAllAsync();
            Machines = await _machineRepo.GetAllAsync();
            foreach (var item in Jobs)
            {
                Frequencies.Add((Frequency)item.FrequencyId);
            }
            //foreach (var machine in Machines)
            //{
            //    // Get jobs for the machine
            //    var machineJobs = Jobs.Where(j => j.MachineId == machine.MachineId).ToList();

            //    // Calculate and set the CardClass for each machine
            //    machine.CardClass = GetCardClass(machine, machineJobs);
            //}
        }
        //private string GetCardClass(Machine machine, List<Job> machineJobs)
        //{
           
        //    if (machine.Status && !machineJobs.Any())
        //    {
        //        return "border-success text-success";
        //    }

        //    if (!machine.Status)
        //    {
        //        return "border-danger text-danger"; 
        //    }
        //    return "border-secondary"; 
        //}
        //public async Task<IActionResult> OnPostDeleteAsync(int id)
        //{
        //    await _jobRepo.DeleteAsync(await _jobRepo.GetByIdAsync(id));
        //    return Page();
        //}
    }
}

