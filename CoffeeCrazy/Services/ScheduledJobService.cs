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
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DateTime lastDailyTask = DateTime.MinValue;
            DateTime lastWeeklyTask = DateTime.MinValue;
            DateTime lastMonthlyTask = DateTime.MinValue;

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                var machines = await _machineRepo.GetAllAsync();
                // Daily jobs
                if (now.Date > lastDailyTask.Date)
                {
                    foreach (var machine in machines)
                    {
                        var dailyJob = new Job
                        {
                            Title = $"Påfyld Kaffe",
                            Description = "",
                            DateCreated = now,
                            Deadline = now.AddDays(1),
                            FrequencyId = 1,
                            MachineId = machine.MachineId
                        };
                        await _jobRepo.CreateAsync(dailyJob);
                    }
                    lastDailyTask = now;
                }

                if (now.Date > lastDailyTask.Date)
                {
                    foreach (var machine in machines)
                    {
                        var dailyJob = new Job
                        {
                            Title = $"Påfyld mælk",
                            Description = "",
                            DateCreated = now,
                            Deadline = now.AddDays(1),
                            FrequencyId = 1,
                            MachineId = machine.MachineId
                        };
                        await _jobRepo.CreateAsync(dailyJob);
                    }
                    lastDailyTask = now;
                }
                if (now.Date > lastDailyTask.Date)
                {
                    foreach (var machine in machines)
                    {
                        var dailyJob = new Job
                        {
                            Title = $"Rengør",
                            Description = "",
                            DateCreated = now,
                            Deadline = now.AddDays(1),
                            FrequencyId = 1,
                            MachineId = machine.MachineId
                        };
                        await _jobRepo.CreateAsync(dailyJob);
                    }
                    lastDailyTask = now;
                }
                // Weekly Jobs
                if (now.DayOfWeek == DayOfWeek.Monday && now.Date > lastWeeklyTask.Date)
                {
                    foreach (var machine in machines)
                    {
                        var weeklyJob = new Job
                        {
                            Title = $"Genfyld kaffekopper",
                            Description = "Husk ikke at sætte flere end 40 stk. op.",
                            DateCreated = now,
                            Deadline = now.AddDays(7),
                            FrequencyId = 2, 
                            MachineId = machine.MachineId
                        };
                        await _jobRepo.CreateAsync(weeklyJob);
                    }
                    lastWeeklyTask = now;
                }
                // Montly job
                if (now.Day == 1 && now.Date > lastMonthlyTask.Date)
                {
                    foreach (var machine in machines)
                    {
                        var monthlyJob = new Job
                        {
                            Title = $"Rens Tube",
                            Description = "Følg Guiden, gør som der bliver sagt.",
                            DateCreated = now,
                            Deadline = now.AddMonths(1),
                            FrequencyId = 3, 
                            MachineId = machine.MachineId
                        };
                        await _jobRepo.CreateAsync(monthlyJob);
                    }
                    lastMonthlyTask = now;
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

    }
}
