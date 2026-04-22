

namespace Application.DTOs
{
    public class MedicineDrawerDto
    {
        public int Id { get; set; }
        public int DrawerNumber { get; set; }
        public bool IsOpened { get; set; }
        public string PatientName { get; set; } = null!;
    }
}
