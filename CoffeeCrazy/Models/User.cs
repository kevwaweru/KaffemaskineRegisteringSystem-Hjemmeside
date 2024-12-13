using CoffeeCrazy.Models.Enums;

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
    }
}
    