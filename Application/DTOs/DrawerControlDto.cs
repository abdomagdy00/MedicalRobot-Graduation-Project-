using Core.Enums;
namespace Application.DTOs
{
    //For drawer opening/closing commands from mobile
    public class DrawerControlDto
    {
        public int DrawerNumber { get; set; }
        public DrawerStatus Status { get; set; }
        //public bool RequestedStatus { get; set; } // True to open, False to close

    }
}
