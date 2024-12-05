using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Services
{
    public class ImageService
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
