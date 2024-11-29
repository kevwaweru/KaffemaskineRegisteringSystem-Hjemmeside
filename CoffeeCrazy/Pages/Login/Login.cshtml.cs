using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Utilitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepo _userRepo;

        public LoginModel(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 

                return Page();

            try
            {
                var (storedHash, storedSalt, role, firstName, userId) = await _userRepo.GetUserByEmailAsync(Email);

                if (PasswordHelper.VerifyPasswordHash(Password, storedHash, storedSalt))
                {
                    
                    HttpContext.Session.SetString("Email", Email);
                    HttpContext.Session.SetInt32("RoleId", (int)role);
                    HttpContext.Session.SetString("FirstName", firstName);
                    HttpContext.Session.SetInt32("UserId",(int)userId);


                    return RedirectToPage("/Index");
                }
                else
                {                    
                    ErrorMessage = "Forkert email eller password.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorMessage = "An error occurred. Please try again.";
            }

            return Page();
        }
    }
}
