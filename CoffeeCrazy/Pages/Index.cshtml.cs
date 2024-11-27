using CoffeeCrazy.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCrazy.Pages
{
    public class IndexModel : PageModel
    {

        ITokenGeneratorRepo _repo;
        public string Email = "wawerukew@gmail.com";
        public string Token;

        public IndexModel(ITokenGeneratorRepo repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            _repo.CreateAsync(Email);

            Token = await _repo.GetTokenAsync(Email);

        }
    }
}
