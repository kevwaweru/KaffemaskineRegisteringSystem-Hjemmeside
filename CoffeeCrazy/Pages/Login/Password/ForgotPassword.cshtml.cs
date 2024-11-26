using CoffeeCrazy.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Login.Password
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserRepo _userRepo;

        public ForgotPasswordModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {

                // en metode der genere en token af en slags
                // en metode der sender email med token

                Message = "En mail med nyt password er sendt til dig.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Der er sket en fejl prøv igen..");
                Console.WriteLine($"Error: {ex.Message}");
            }

            return Page();
        }
    }
}
