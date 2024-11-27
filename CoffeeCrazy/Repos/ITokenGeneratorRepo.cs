namespace CoffeeCrazy.Repos
{
    public interface ITokenGeneratorRepo
    {
        Task CreateAsync(string email);
        Task<int?> GetTokenAsync(string email);
    }
}