namespace PetGroomingApp.Web.ViewModels.Appointment
{
    public class AppointmentListViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime AppointmentTime { get; set; }

        public string? GroomerName { get; set; }

        public string? PetName { get; set; }

        public string? OwnerName { get; set; }

        public string Status { get; set; } = null!;
    }

}
