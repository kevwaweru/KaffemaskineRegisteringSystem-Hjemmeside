using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        private readonly IAccessService _accessService;

        public LoginModel(IUserRepo userRepo, IAccessService accessService)
        {
            _userRepo = userRepo;
            _accessService = accessService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
         
            if (_accessService.IsUserLoggedIn(HttpContext))
                return RedirectToPage("/Machines/Index"); 
            
            return Page();
        }

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


                    return RedirectToPage("/Machines/Index");
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
