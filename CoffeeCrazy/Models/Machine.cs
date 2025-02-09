using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Services;

namespace CoffeeCrazy.Models
{
    public class Machine
    {
        public int MachineId { get; set; }
        public bool Status { get; set; }
        public string? Placement { get; set; }
        public Campus Campus { get; set; }
        public IFormFile? MachineImage { get; set; } //nullable


        public Machine(int machineId, bool status, string? placement, Campus campus, byte[]? machineImage)
        {
            MachineId = machineId;
            Status = status;
            Placement = placement;
            Campus = campus;
            if (machineImage != null)
            {
                MachineImage = ImageService.ByteArrayToFormFile(machineImage);
            }

        }

    }
}
