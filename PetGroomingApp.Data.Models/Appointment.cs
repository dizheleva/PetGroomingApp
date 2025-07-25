﻿namespace PetGroomingApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models.Enums;

    [Comment("Appointment in the system")]
    public class Appointment
    {
        [Comment("Appointment identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Time of Appointment creation")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Comment("Appointment date")]
        public DateTime Date { get; set; }

        [Comment("Appointment notes")]
        public string? Notes { get; set; }

        [Comment("Foreign key to the pet for the appointment")]
        public Guid? PetId { get; set; }
        public virtual Pet? Pet { get; set; }

        [Comment("Foreign key to the user for the appointment")]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;

        [Comment("Foreign key to the groomer for the appointment")]
        public Guid? GroomerId { get; set; }
        public virtual Groomer? Groomer { get; set; }

        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
            = new HashSet<AppointmentService>();

        public AppointmentStatus Status { get; set; } 

    }

}
