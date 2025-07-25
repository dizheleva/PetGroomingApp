namespace PetGroomingApp.Web.ViewModels.Groomer
{
    using System.ComponentModel.DataAnnotations;

    using static PetGroomingApp.Services.Common.EntityConstants.Groomer;

    public class GroomerFormViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = FirstNameRequiredMessage)]
        [MinLength(FirstNameMinLength, ErrorMessage = FirstNameMinLengthMessage)]
        [MaxLength(FirstNameMaxLength, ErrorMessage = FirstNameMaxLengthMessage)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = LastNameRequiredMessage)]
        [MinLength(LastNameMinLength, ErrorMessage = LastNameMinLengthMessage)]
        [MaxLength(LastNameMaxLength, ErrorMessage = LastNameMaxLengthMessage)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = JobTitleRequiredMessage)]
        [MaxLength(JobTitleMaxLength, ErrorMessage = JobTitleMaxLengthMessage)]
        public required string JobTitle { get; set; }

        [Required(ErrorMessage = ImageUrlRequiredMessage)]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public required string ImageUrl { get; set; }

        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string? Description { get; set; }

        [Required(ErrorMessage = PhoneNumberRequiredMessage)]
        [RegularExpression(PhoneNumberPattern, ErrorMessage = PhoneNumberInvalidMessage)]
        public required string PhoneNumber { get; set; }

    }
}
