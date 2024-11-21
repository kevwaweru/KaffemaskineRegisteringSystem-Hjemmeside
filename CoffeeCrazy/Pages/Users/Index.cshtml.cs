using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ICRUDRepo<User> _UserCRUDRepo;
        public IndexModel(ICRUDRepo<User> userCrudRepository)
        {
            _UserCRUDRepo = userCrudRepository;
        }

        public List<User> Users { get; private set; }

        public async Task OnGetAsync()
        {
            Users = await _UserCRUDRepo.GetAllAsync();      
        }
    }
}
