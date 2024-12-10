using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;

namespace CoffeeCrazy.Services
{
    public class ScheduledJobService : BackgroundService
    {
        private readonly ICRUDRepo<Job> _jobRepo;
        private readonly ICRUDRepo<Machine> _machineRepo;

        public ScheduledJobService(ICRUDRepo<Job> jobRepo, ICRUDRepo<Machine> machineRepo)
        {
            _jobRepo = jobRepo;
            _machineRepo = machineRepo;
        }

        /// <summary>
        /// Executes the background service to create jobs based on the Enum Frequencies (daily, weekly, monthly).
        /// Runs every day at 07:00 UTC and creates jobs for all machines.
        /// </summary>
        /// <param name="stoppingToken">Token used to cancel the background task.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (now.Hour == 7 && now.Minute == 0)
                {
                    await CreateJobsForTodayAsync(now);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); //Hver minut
          //      await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // hver Time
            //    await Task.Delay(TimeSpan.FromHours(24), stoppingToken); /// Hver dag

            }
        }

        /// <summary>
        /// Creates jobs for the current day based on job frequencies (daily, weekly, monthly).
        /// </summary>
        /// <param name="currentTime">The current UTC time.</param>
        private async Task CreateJobsForTodayAsync(DateTime currentTime)
        {
            var machines = await _machineRepo.GetAllAsync();

            if (currentTime.DayOfWeek != DayOfWeek.Saturday && currentTime.DayOfWeek != DayOfWeek.Sunday)
            {
                await CreateDailyJobsAsync(machines, currentTime);
            }

            if (currentTime.DayOfWeek == DayOfWeek.Monday)
            {
                await CreateWeeklyJobsAsync(machines, currentTime);
            }

            if (currentTime.Day == 1)
            {
                await CreateMonthlyJobsAsync(machines, currentTime);
            }
        }

        /// <summary>
        /// Creates daily jobs for all machines.
        /// </summary>
        /// <param name="machines">The list of machines.</param>
        /// <param name="currentTime">The current UTC time.</param>
        private async Task CreateDailyJobsAsync(IEnumerable<Machine> machines, DateTime currentTime)
        {
            foreach (var machine in machines)
            {
                var dailyJobs = new List<Job>
        {
            new Job
            {
                Title = "Påfyld Kaffe",
                Description = "",
                DateCreated = currentTime,
                Deadline = currentTime.AddDays(1),
                FrequencyId = 1,
                MachineId = machine.MachineId
            },
            new Job
            {
                Title = "Påfyld mælk",
                Description = "",
                DateCreated = currentTime,
                Deadline = currentTime.AddDays(1),
                FrequencyId = 1,
                MachineId = machine.MachineId
            },
            new Job
            {
                Title = "Rengør",
                Description = "",
                DateCreated = currentTime,
                Deadline = currentTime.AddDays(1),
                FrequencyId = 1,
                MachineId = machine.MachineId
            }
        };

                foreach (var job in dailyJobs)
                {
                    await _jobRepo.CreateAsync(job);
                }
            }
        }

        /// <summary>
        /// Creates weekly jobs for all machines.
        /// </summary>
        /// <param name="machines">The list of machines.</param>
        /// <param name="currentTime">The current UTC time.</param>
        private async Task CreateWeeklyJobsAsync(IEnumerable<Machine> machines, DateTime currentTime)
        {
            foreach (var machine in machines)
            {
                var weeklyJob = new Job
                {
                    Title = "Genfyld kaffekopper",
                    Description = "Husk ikke at sætte flere end 40 stk. op.",
                    DateCreated = currentTime,
                    Deadline = currentTime.AddDays(7),
                    FrequencyId = 2,
                    MachineId = machine.MachineId
                };
                await _jobRepo.CreateAsync(weeklyJob);
            }
        }

        /// <summary>
        /// Creates monthly jobs for all machines.
        /// </summary>
        /// <param name="machines">The list of machines.</param>
        /// <param name="currentTime">The current UTC time.</param>
        private async Task CreateMonthlyJobsAsync(IEnumerable<Machine> machines, DateTime currentTime)
        {
            foreach (var machine in machines)
            {
                var monthlyJob = new Job
                {
                    Title = "Rens Tube",
                    Description = "Følg Guiden, gør som der bliver sagt.",
                    DateCreated = currentTime,
                    Deadline = currentTime.AddMonths(1),
                    FrequencyId = 3,
                    MachineId = machine.MachineId
                };
                await _jobRepo.CreateAsync(monthlyJob);
            }
        }
    }
}
