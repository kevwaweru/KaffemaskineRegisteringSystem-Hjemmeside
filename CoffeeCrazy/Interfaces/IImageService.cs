﻿namespace CoffeeCrazy.Interfaces
{
    public interface IImageService
    {
        byte[] ConvertImageToByteArray(IFormFile imageFile);
    }
}
