namespace CoffeeCrazy.Models.Enums
{
    //Gorm
    public enum Role
    {
        MasterAdmin = 1,
        Admin = 2,
        [Display(Name = "Medarbejder")]
        Employee = 3,
    }
}
