using CoffeeCrazy.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CoffeeCrazy.Models
{
    //Kevin
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public Campus Campus { get; set; }
        public IFormFile? UserImage { get; set; }
        public string? UserImageBase64
        {
            get
            {
                if (UserImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        UserImage.CopyTo(memoryStream);
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                return null;
            }
        }
    }
}
    