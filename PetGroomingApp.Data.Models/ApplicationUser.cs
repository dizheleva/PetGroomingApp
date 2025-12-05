namespace PetGroomingApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual Groomer? Groomer { get; set; }

        public virtual ICollection<Pet>? Pets { get; set; } = new HashSet<Pet>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new HashSet<Appointment>();
    }
}
