using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Model
{
    //Kevin
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string Passowrd { get; set; }
        public Role Role { get; set; }
        public Campus Campus { get; set; }
    }
}
    