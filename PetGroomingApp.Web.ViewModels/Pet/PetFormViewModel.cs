namespace PetGroomingApp.Web.ViewModels.Pet
{
    using System.ComponentModel.DataAnnotations;
    using PetGroomingApp.Data.Models.Enums;

    using static PetGroomingApp.Services.Common.EntityConstants.Pet;

    public class PetFormViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = TypeRequiredMessage)]
        [EnumDataType(typeof(PetType), ErrorMessage = TypeInvalidMessage)]
        [Display(Name = "Pet Type")]
        public PetType Type { get; set; }

        [Required(ErrorMessage = BreedRequiredMessage)]
        [MinLength(BreedMinLength, ErrorMessage = BreedMinLengthMessage)]
        [MaxLength(BreedMaxLength, ErrorMessage = BreedMaxLengthMessage)]
        public string Breed { get; set; } = null!;
        
        [Required(ErrorMessage = SizeRequiredMessage)]
        [EnumDataType(typeof(PetSize), ErrorMessage = SizeInvalidMessage)]
        [Display(Name = "Pet Size")]
        public PetSize Size { get; set; }

        [Required(ErrorMessage = GenderRequiredMessage)]
        [EnumDataType(typeof(PetGender), ErrorMessage = GenderInvalidMessage)]
        public PetGender Gender { get; set; }

        [Required(ErrorMessage = AgeRequiredMessage)]
        [Range(AgeMinValue, AgeMaxValue, ErrorMessage = AgeRangeMessage)]
        public int Age { get; set; }

        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [MinLength(NotesMinLength, ErrorMessage = NotesMinLengthMessage)]
        [MaxLength(NotesMaxLength, ErrorMessage = NotesMaxLengthMessage)]
        [Display(Name = "Additional Notes")]
        public string? Notes { get; set; }
                
        [Display(Name = "Owner ID")]
        [RegularExpression(@"^[a-zA-Z0-9-]{36}$", ErrorMessage = OwnerIdInvalidMessage)]
        public string? OwnerId { get; set; }
    }
}
