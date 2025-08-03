namespace PetGroomingApp.WebApi.Dtos
{
    public class AppointmentCalculationDto
    {
        public int TotalDuration { get; set; } // minutes
        public decimal TotalPrice { get; set; }
    }
}
