using CoffeeCrazy.Model;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly UserRepo _userRepository;

        public List<User> Users { get; private set; }
        // ville bruge det til at tage id med. men det bliver jo httpContext der klare det.
      //  public User SelectedUser { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; } 

        public IndexModel(UserRepo userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task OnGetAsync()
        {

            Users = await _userRepository.GetAllAsync();

   //en del af Tanken men det der Seleted User.
            //if (Id.HasValue)
            //{
            //    try
            //    {
            //        SelectedUser = await _userRepository.GetByIdAsync(Id.Value);
            //    }
            //    catch
            //    {
             
            //        SelectedUser = null;
            //    }
            //}
        }
    }
}
