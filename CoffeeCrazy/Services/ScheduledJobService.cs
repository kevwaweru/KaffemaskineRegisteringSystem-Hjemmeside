using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;

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

                if (now.Hour == 07 && now.Minute == 00)
                {
                    await CreateJobsForTodayAsync(now);
                }


                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);


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
                var allJobs = await _jobRepo.GetAllAsync();
                var dailyJobs = allJobs.Where(j => j.MachineId == machine.MachineId && j.FrequencyId == 1).ToList();
                var groupedDailyJobs = dailyJobs.GroupBy(j => j.Title).ToList();

                foreach (var jobGroup in groupedDailyJobs)
                {
                    var job = jobGroup.First();

                    var newJob = new Job
                    {
                        Title = job.Title,
                        Description = job.Description,
                        Comment = job.Comment,
                        IsCompleted = false,
                        DateCreated = currentTime,
                        Deadline = currentTime.AddDays(1),
                        FrequencyId = 1,
                        MachineId = machine.MachineId,
                        UserId = job.UserId,
                    };
                    await _jobRepo.CreateAsync(newJob);
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
                var allJobs = await _jobRepo.GetAllAsync();
                var weeklyJobs = allJobs.Where(j => j.MachineId == machine.MachineId && j.FrequencyId == 1).ToList();
                var groupedWeeklyJobs = weeklyJobs.GroupBy(j => j.Title).ToList();

                foreach (var jobGroup in groupedWeeklyJobs)
                {
                    var job = jobGroup.First();

                    var newJob = new Job
                    {
                        Title = job.Title,
                        Description = job.Description,
                        Comment = job.Comment,
                        IsCompleted = false,
                        DateCreated = currentTime,
                        Deadline = currentTime.AddDays(7),
                        FrequencyId = 2,
                        MachineId = machine.MachineId,
                        UserId = job.UserId,
                    };
                    await _jobRepo.CreateAsync(newJob);
                }
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
                var allJobs = await _jobRepo.GetAllAsync();
                var monthlyJob = allJobs.Where(j => j.MachineId == machine.MachineId && j.FrequencyId == 1).ToList();
                var groupedWeeklyJobs = monthlyJob.GroupBy(j => j.Title).ToList();
                foreach (var jobGroup in groupedWeeklyJobs)
                {
                    var job = jobGroup.First();

                    var newJob = new Job
                    {
                        Title = job.Title,
                        Description = job.Description,
                        Comment = job.Comment,
                        IsCompleted = false,
                        DateCreated = currentTime,
                        Deadline = currentTime.AddDays(7),
                        FrequencyId = 2,
                        MachineId = machine.MachineId,
                        UserId = null,
                    };
                    await _jobRepo.CreateAsync(newJob);
                }
            }
        }
    }
}
