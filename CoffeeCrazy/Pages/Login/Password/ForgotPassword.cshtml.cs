using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Dynamic;

namespace CoffeeCrazy.Pages.Login.Password
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IAccessService _accessService;

        [BindProperty]
        public string Email { get; set; }
        public string Message { get; set; }

        public ForgotPasswordModel( IEmailService emailService, IAccessService accessService)
        {
            _emailService = emailService;
            _accessService = accessService;
        }

        public IActionResult OnGet()
        {
            if (_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Machines/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("","Venligst intast email.");
                return Page(); 
            }   
               
               bool emailSent = await _emailService.GenerateTokenAndSendResetEmail(email);

            if (emailSent)
            {
                Message = "En mail med nyt password er sendt til dig.";
  
                return RedirectToPage("/Login/Password/ResetPassword");
            }
            else
            {
                Message = "Der er sket en fejl Token bliv ikke sendt prøv igen";


                return Page();
            }                  
        }
    }
}
