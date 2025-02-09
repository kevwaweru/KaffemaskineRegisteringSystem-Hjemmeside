using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Services;

namespace CoffeeCrazy.Models
{
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
        public IFormFile? UserImageFile { get; set; }

        public User(int userId, string firstName, string lastName, string email, Role role, Campus campus, byte[]? userImageFile)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            Campus = campus;
            if (userImageFile != null)
            {
                UserImageFile = ImageService.ByteArrayToFormFile(userImageFile);
            }
        }
    }
}
    