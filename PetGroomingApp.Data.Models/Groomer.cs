namespace PetGroomingApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    [Comment("Groomer in the system")]
    public class Groomer
    {
        [Comment("Groomer identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Groomer first name")]
        [Required]
        public required string FirstName { get; set; }

        [Comment("Groomer last name")]
        [Required]
        public required string LastName { get; set; }

        [Comment("Groomer phone number")]
        public string? PhoneNumber { get; set; }

        [Comment("Any description of the Groomer ")]
        public string? Description { get; set; } // Optional bio or specialization

        [Comment("Shows if Groomer is no longer working")]
        public bool IsDeleted { get; set; } = false;

        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }

}
