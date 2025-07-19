namespace PetGroomingApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Service in the system")]
    public class Service
    {
        [Comment("Service identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Service name")]
        public required string Name { get; set; }

        [Comment("Service description")]
        public string? Description { get; set; }

        [Comment("Service duration")]
        public required TimeSpan Duration { get; set; }

        [Comment("Service price")]
        public decimal Price { get; set; }

        [Comment("Shows if Service is deleted")]
        public bool IsDeleted { get; set; } = false;

        // Navigation
        public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new HashSet<AppointmentService>();
    }

}
