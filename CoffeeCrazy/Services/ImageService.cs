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
                //imageFile.FileName.ToString();
                imageFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public IFormFile ConvertArrayToIFormFile(byte[] imageByte)
        {

            FormFile file = new FormFile(new MemoryStream(imageByte), 0, imageByte.Length, "file", "image.jpg");
            return file;

        }
    }
}
