namespace PetGroomingApp.Data.Models
{
    public class AppointmentService
    {
        public Guid AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; } = null!;

        public Guid ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;
    }
}
