namespace PetGroomingApp.Web.ViewModels.Appointment
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PetGroomingApp.Data.Models.Enums;

    public class AppointmentFormViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date and Time")]
        public DateTime AppointmentTime { get; set; }

        [Required]  
        [Display(Name = "Services")]
        public List<Guid> SelectedServiceIds { get; set; } = [];
        
        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        [Display(Name = "Pet")]
        public Guid? PetId { get; set; }

        [Display(Name = "Groomer")]
        public Guid? GroomerId { get; set; }

        [Display(Name = "User")]
        public string? UserId { get; set; }

        [Display(Name = "Status")]
        public AppointmentStatus Status{ get; set; }
                
        // For dropdowns
        public IEnumerable<SelectListItem>? Pets { get; set; }
        public IEnumerable<SelectListItem>? Groomers { get; set; }
        public IEnumerable<SelectListItem>? Services { get; set; }
        public IEnumerable<SelectListItem>? Statuses { get; set; }

        [Display(Name = "Total Duration (minutes)")]
        public int TotalDuration { get; set; }

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]

        public decimal TotalPrice { get; set; }
    }
}
