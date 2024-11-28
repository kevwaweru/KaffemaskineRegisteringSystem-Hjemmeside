namespace CoffeeCrazy.Repos
{
    public interface ITokenGeneratorRepo
    {
        Task CreateAsync(string email);
        Task<string> GetTokenAsync(string email);
        Task<bool> ValidateTokenAsync(string token);
        Task DeleteAsync(string token);
    }
}