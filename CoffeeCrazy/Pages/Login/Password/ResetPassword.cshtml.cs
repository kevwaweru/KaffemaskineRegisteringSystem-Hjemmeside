using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Repos;
using CoffeeCrazy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeCrazy.Pages.Login.Password
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly ITokenRepo _tokenGeneratorRepo;
        private readonly IAccessService _accessService;
        public ResetPasswordModel(IUserRepo userRepo, ITokenRepo tokenGeneratorRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _tokenGeneratorRepo = tokenGeneratorRepo;
            _accessService = accessService;
        }
        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Du har ikke skrevet det samme.")]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        [TempData]
        public bool IsTokenValidated { get; set; } = false; // det der tempData er jeg bange for kommer til at fucke med programmet. Har haft bugs hvor den giver mig en "ValidToken" uden jeg har intastet en. og kommer derekte videre til else statement :(
        public string Message { get; set; }       
        
        public IActionResult OnGet()
        {
            if (_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Machines/Index"); // Skal sende folk til main siden
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"IsTokenValidated: {IsTokenValidated}");
            //Første del af if statement i HTML.
            if (!IsTokenValidated)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    ModelState.AddModelError("", "Indsæt en gyldig engangskode.");
                    return Page();
                }

                bool isValid = await _tokenGeneratorRepo.ValidateTokenAsync(Token);
                if (isValid)
                {
                    IsTokenValidated = true;
                    return Page();
                }
                else
                {
                    Message = "Forkert eller udløbet engangskode.";
                    return Page();
                }             
            }
            //Anden del af if statement
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool isStillValid = await _tokenGeneratorRepo.ValidateTokenAsync(Token);
            if (!isStillValid)
            {
                ModelState.AddModelError("", "Engangskoden er udløbet. Ansøg om en ny.");
                Thread.Sleep(1000);
                return RedirectToPage("/Login/Password/ForgotPassword");
            }

            try
            {
                bool isResetSuccessful = await _userRepo.ResetPasswordAsync(Token, NewPassword);

                if (isResetSuccessful)
                {
                    Message = "Nyt kodeord er opretter du bliver diregeret til Login siden.";
                    Thread.Sleep(2000);
                    return RedirectToPage("/Login/Login");
                }
                else
                {
                    ModelState.AddModelError("", "Udløbet engangskode. Ansøg om ny");
                    Thread.Sleep(2000);
                    return RedirectToPage("/Login/Password/ForgotPassword");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "Der er sket en fejl..");
            }

            return Page();
        }
    }
}
