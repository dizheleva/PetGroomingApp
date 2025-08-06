namespace PetGroomingApp.Web.ViewModels.Appointment
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AppointmentManagerFormViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date and Time")]
        public DateTime AppointmentTime { get; set; }

        [Display(Name = "User")]
        public string? UserId { get; set; }
        public List<SelectListItem> Users { get; set; } = new();

        [Display(Name = "Pet")]
        public Guid? SelectedPetId { get; set; }
        public List<SelectListItem> Pets { get; set; } = new();

        [Required]
        [Display(Name = "Groomer")]
        public Guid? SelectedGroomerId { get; set; }
        public List<SelectListItem> Groomers { get; set; } = new();

        [Required]
        [Display(Name = "Services")]
        public List<Guid> SelectedServiceIds { get; set; } = new();
        public List<SelectListItem> Services { get; set; } = new();

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;
        public List<SelectListItem> Statuses { get; set; } = new();

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Total Duration (minutes)")]
        public int TotalDuration { get; set; }

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }
    }
}
