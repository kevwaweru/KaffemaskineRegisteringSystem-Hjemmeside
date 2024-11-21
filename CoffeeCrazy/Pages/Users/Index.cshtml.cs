using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepo _UserRepo;
        public IndexModel(IUserRepo userCrudRepository)
        {
            _UserRepo = userCrudRepository;
        }

        public List<User> Users { get; private set; }

        public async Task OnGetAsync()
        {
            Users = await _UserRepo.GetAllAsync();      
        }
    }
}
