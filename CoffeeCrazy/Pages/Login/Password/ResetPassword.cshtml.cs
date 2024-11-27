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

        //[BindProperty]
        //public string Email { get; set; }

        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }
        public bool IsTokenValidated { get; set; } = false;
        // den første del af If statement i HTML.
        public async Task<IActionResult> OnPostValidateTokenAsync()
        {
            if (string.IsNullOrEmpty(Token))
            {
                ModelState.AddModelError("", "Please enter a valid token.");
                return Page();
            }

            bool isValid = await _tokenGeneratorRepo.ValidateTokenAsync(Token);
            if (isValid)
            {
                IsTokenValidated = true;
            }
            else
            {
                Message = "Forkert eller udløbet engangskode.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                bool isResetSuccessful = await _userRepo.ResetPasswordAsync(Token, NewPassword);

                if (isResetSuccessful)
                {
                    Message = "Your password has been reset successfully.";
                    Thread.Sleep(2000);
                    return RedirectToPage("/Login/Login");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid or expired token.");
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
