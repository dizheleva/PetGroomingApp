namespace PetGroomingApp.Web.ViewModels.Groomer
{
    using System.ComponentModel.DataAnnotations;

    using static PetGroomingApp.GCommon.Constants.GroomerConstants;
    using static PetGroomingApp.GCommon.Messages.GroomerValidationMessages;

    public class GroomerFormViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = FirstNameRequiredMessage)]
        [MinLength(FirstNameMinLength, ErrorMessage = FirstNameMinLengthMessage)]
        [MaxLength(FirstNameMaxLength, ErrorMessage = FirstNameMaxLengthMessage)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = LastNameRequiredMessage)]
        [MinLength(LastNameMinLength, ErrorMessage = LastNameMinLengthMessage)]
        [MaxLength(LastNameMaxLength, ErrorMessage = LastNameMaxLengthMessage)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = JobTitleRequiredMessage)]
        [MaxLength(JobTitleMaxLength, ErrorMessage = JobTitleMaxLengthMessage)]
        public string JobTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = ImageUrlRequiredMessage)]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public string ImageUrl { get; set; } = string.Empty;

        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string? Description { get; set; }

        [Required(ErrorMessage = PhoneNumberRequiredMessage)]
        [RegularExpression(PhoneNumberPattern, ErrorMessage = PhoneNumberInvalidMessage)]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
