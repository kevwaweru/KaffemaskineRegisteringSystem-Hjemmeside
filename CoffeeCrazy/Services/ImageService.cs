using CoffeeCrazy.Interfaces;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Services
{
    public class ImageService : IImageService
    {
        // Convert IFormFile to byte array
        public byte[] ConvertImageToByteArray(IFormFile imageFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                imageFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
