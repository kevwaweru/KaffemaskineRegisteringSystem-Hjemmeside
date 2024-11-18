using System.ComponentModel.DataAnnotations;

namespace CoffeeCrazy.Models.Enums
{
    //Gorm
    public enum Role
    {

        [Display(Name = "MasterAdmin")]
        MasterAdmin = 1,

        [Display(Name = "Admin")]
        Admin = 2,

        [Display(Name = "Medarbejder")]
        Employee = 3,
    }
}
