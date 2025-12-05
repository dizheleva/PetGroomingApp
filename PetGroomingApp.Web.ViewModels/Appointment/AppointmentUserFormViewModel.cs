namespace PetGroomingApp.Web.ViewModels.Appointment
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    public class AppointmentUserFormViewModel
    {
        public Guid Id { get; set; } 

        [Required]
        [Display(Name = "Appointment Time")]
        public DateTime AppointmentTime { get; set; } = DateTime.Today.AddHours(9);

        [Display(Name = "User (optional - for unregistered customers)")]
        public string? UserId { get; set; }
        public List<SelectListItem> Users { get; set; } = new();

        [Display(Name = "Pet")]
        public Guid? SelectedPetId { get; set; }
        public List<SelectListItem> Pets { get; set; } = new();

        [Display(Name = "Pet Name (required if no pet selected)")]
        public string? PetName { get; set; }

        [Display(Name = "Groomer")]
        public Guid? SelectedGroomerId { get; set; }
        public List<SelectListItem> Groomers { get; set; } = new();

        [Required]
        [Display(Name = "Services")]
        public List<Guid>? SelectedServiceIds { get; set; }
        public List<SelectListItem> Services { get; set; } = new();

        public string? Status { get; set; } 

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Total Duration (minutes)")]
        public TimeSpan TotalDuration { get; set; } = TimeSpan.Zero;

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; } = 0;
    }
}
