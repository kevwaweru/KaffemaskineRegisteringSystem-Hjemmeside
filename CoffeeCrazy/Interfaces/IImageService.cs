namespace CoffeeCrazy.Interfaces
{
    public interface IImageService
    {
        byte[] FormFileToByteArray(IFormFile imageFile);

        IFormFile ByteArrayToFormFile(byte[] imageByte);

        string? FormFileToBase64String(IFormFile imageFile);
    }
}
