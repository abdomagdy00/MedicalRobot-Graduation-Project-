
using Core.Enums;

namespace Core.Entities
{
    public class MedicineDrawer
    {
        public int Id { get; set; }
        public int DrawerNumber { get; set; }
        public bool IsOpened { get; set; }
        public string CommandChar { get; set; } = null!;
        public DrawerStatus DrawerStatus { get; set; }
        public int? PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}   
