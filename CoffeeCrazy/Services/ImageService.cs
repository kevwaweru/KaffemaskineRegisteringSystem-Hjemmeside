using CoffeeCrazy.Interfaces;

namespace CoffeeCrazy.Services
{
    public class ImageService
    {
        // Convert IFormFile to byte array
        public byte[] FormFileToByteArray(IFormFile imageFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                //imageFile.FileName.ToString();
                imageFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static IFormFile ByteArrayToFormFile(byte[] imageByte)
        {

            FormFile file = new FormFile(new MemoryStream(imageByte), 0, imageByte.Length, "file", "image.jpg");
            return file;

        }

        public string? FormFileToBase64String(IFormFile imageFile)
        {

            if (imageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.CopyTo(memoryStream);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return null;
        }
    }
}
