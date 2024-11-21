using CoffeeCrazy.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoffeeCrazy.Model
{
    //Kevin
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public Campus Campus { get; set; }
    }
}
    