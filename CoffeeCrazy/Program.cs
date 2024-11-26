using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Repos;
using CoffeeCrazy.Services;

namespace CoffeeCrazy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();   
            // repositories
            builder.Services.AddTransient<IUserRepo, UserRepo>();    
            builder.Services.AddTransient<IAssignmentRepo, AssignmentRepo>();
            builder.Services.AddTransient<IAssignmentSetRepo, AssignmentSetRepo>();
            // Services
            builder.Services.AddTransient<IEmailService, EmailService>();
            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true; // Makes Cookies Safe
                options.Cookie.IsEssential = true; // Reqires "GDPR-samtykke"
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
         

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
