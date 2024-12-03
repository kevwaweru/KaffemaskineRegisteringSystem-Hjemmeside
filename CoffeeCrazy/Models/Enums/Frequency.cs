using System.ComponentModel.DataAnnotations;

namespace CoffeeCrazy.Models.Enums
{
    public enum Frequency
    {
        [Display(Name = "Daglig")]
        Daily = 1,
        [Display(Name = "Ugentligt")]
        Weekly = 2,
        [Display(Name = "Måndenligt")]
        Monthly = 3
    }
}
