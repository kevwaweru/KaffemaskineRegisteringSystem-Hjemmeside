namespace CoffeeCrazy.Repos
{
    public interface ITokenRepo
    {
        Task CreateTokenAsync(string email);
        Task<string> GetTokenAsync(string email);
        Task<bool> ValidateTokenAsync(string token);
        Task DeleteTokenAsync(string token);
    }
}