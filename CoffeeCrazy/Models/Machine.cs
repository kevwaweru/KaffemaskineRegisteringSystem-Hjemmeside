namespace CoffeeCrazy.Model
{
    public class Machine
    {
        public int MachineId { get; set; }
        public bool Status { get; set; }        
        public int CampusId { get; set; }
        public string? Placement { get; set; }
    }
}
