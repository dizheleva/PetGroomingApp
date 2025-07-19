namespace PetGroomingApp.Data.Models
{
    public class AppointmentService
    {
        public Guid AppointmentId { get; set; }
        public virtual required Appointment Appointment { get; set; }

        public Guid ServiceId { get; set; }
        public virtual required Service Service { get; set; }
    }
}
