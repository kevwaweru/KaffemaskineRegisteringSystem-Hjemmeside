using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeCrazy.Pages.Login.Password
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IPasswordRepo _passwordRepo;
        private readonly ITokenRepo _tokenGeneratorRepo;
        private readonly IAccessService _accessService;

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
        public bool IsTokenValidated { get; set; } = false;
        public string Message { get; set; }

        public ResetPasswordModel(IPasswordRepo passwordRepo, ITokenRepo tokenGeneratorRepo, IAccessService accessService)
        {
            _passwordRepo = passwordRepo;
            _tokenGeneratorRepo = tokenGeneratorRepo;
            _accessService = accessService;
        }      
        
        public IActionResult OnGet()
        {
            if (_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Machines/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"IsTokenValidated: {IsTokenValidated}");
            //First part of the If statement
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
            //second part of the if statement
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool isStillValid = await _tokenGeneratorRepo.ValidateTokenAsync(Token);
            if (!isStillValid)
            {
                ModelState.AddModelError("", "Engangskoden er udløbet. Ansøg om en ny.");
                return RedirectToPage("/Login/Password/ForgotPassword");
            }

            try
            {
                bool isResetSuccessful = await _passwordRepo.ResetPasswordAsync(Token, NewPassword);

                if (isResetSuccessful)
                {
                    return RedirectToPage("/Login/Login");
                }
                else
                {
                    ModelState.AddModelError("", "Udløbet engangskode. Ansøg om ny");
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
