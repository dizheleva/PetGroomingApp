namespace PetGroomingApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    [Comment("Groomer in the system")]
    public class Groomer
    {
        [Comment("Groomer identifier")]
        public Guid Id { get; set; }

        [Comment("Groomer first name")]
        [Required]
        public string FirstName { get; set; } = null!;

        [Comment("Groomer last name")]
        [Required]
        public string LastName { get; set; } = null!;

        [Comment("Groomer job title")]
        [Required]
        public string JobTitle { get; set; } = null!;

        [Comment("Groomer image URL")]
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Comment("Groomer phone number")]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!; 

        [Comment("Any description of the Groomer ")]
        public string? Description { get; set; } // Optional bio or specialization

        [Comment("Shows if Groomer is no longer working")]
        public bool IsDeleted { get; set; } = false;

        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }

}
