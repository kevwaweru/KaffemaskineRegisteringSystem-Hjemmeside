﻿namespace CoffeeCrazy.Repos
{
    public interface ITokenGeneratorRepo
    {
        Task CreateAsync(string email);
        Task<string> GetTokenAsync(string email);
    }
}