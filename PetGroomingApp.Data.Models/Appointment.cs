namespace PetGroomingApp.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models.Enums;

    [Comment("Appointment in the system")]
    public class Appointment
    {
        [Comment("Appointment identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Time of Appointment creation")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Comment("Appointment date and time")]
        public DateTime AppointmentTime { get; set; }

        [Comment("Appointment duration")]
        public TimeSpan Duration { get; set; }

        [Comment("Appointment notes")]
        public string? Notes { get; set; }

        [Comment("Foreign key to the pet for the appointment")]
        public Guid? PetId { get; set; }
        public virtual Pet? Pet { get; set; }

        [Comment("Foreign key to the user for the appointment")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; } = null!;

        [Comment("Foreign key to the groomer for the appointment")]
        public Guid? GroomerId { get; set; }
        public virtual Groomer? Groomer { get; set; }

        [Comment("Collection of services for the appointment")]
        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
            = new HashSet<AppointmentService>();

        [Comment("Total price of appointment")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Comment("Appointment status")]
        public AppointmentStatus Status { get; set; } 

    }

}
