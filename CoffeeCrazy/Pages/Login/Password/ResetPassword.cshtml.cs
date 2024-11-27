using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeCrazy.Pages.Login.Password
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly ITokenGeneratorRepo _tokenGeneratorRepo;
        public ResetPasswordModel(IUserRepo userRepo, ITokenGeneratorRepo tokenGeneratorRepo)
        {
            _userRepo = userRepo;
            _tokenGeneratorRepo = tokenGeneratorRepo;
        }
        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        [Required]
        public string NewPassword { get; set; }

        [BindProperty]
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Du har ikke skrevet det samme...")]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public bool IsTokenValidated { get; set; } = false;
        public string Message { get; set; }       
        
        public async Task<IActionResult> OnPostAsync()
        {
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
