namespace PetGroomingApp.Web.ViewModels.Appointment
{
    public class AppointmentDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime AppointmentTime { get; set; }

        public int Duration { get; set; }

        public string? Notes { get; set; }

        public string? GroomerName { get; set; }

        public string? PetName { get; set; }

        public string? OwnerName { get; set; }

        public List<string> Services { get; set; } = [];

        public string Status { get; set; } = null!;

        public decimal Price { get; set; }
    }

}
