using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Models
{
    public class Machine
    {
        public int MachineId { get; set; }
        public bool Status { get; set; }        
        public Campus Campus { get; set; }
        public string? Placement { get; set; }
    }
}
