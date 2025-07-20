namespace PetGroomingApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models.Enums;

    using static PetGroomingApp.Data.Common.EntityConstansts.PetConstants;

    [Comment("Pet entity in the system")]
    public class Pet
    {
        [Comment("Pet Identifier")]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Pet name")]
        [Required(ErrorMessage = RequiredNameErrorMessage)]
        [StringLength(PetNameMaxLength, ErrorMessage = PetNameMaxLengthErrorMessage)]
        public required string Name { get; set; }

        [Comment("Pet type")]
        [Required(ErrorMessage = RequiredPetTypeErrorMessage)]
        public required PetType Type { get; set; }

        [Comment("Pet breed")]
        [Required(ErrorMessage = RequiredBreedErrorMessage)]
        [StringLength(BreedMaxLength, ErrorMessage =
            BreedMaxLengthErrorMessage)]
        public required string Breed { get; set; }

        [Comment("Pet size")]
        [Required(ErrorMessage = RequiredPetSizeErrorMessage)]
        public required PetSize Size { get; set; }

        [Comment("Pet age")]
        [Required(ErrorMessage = RequiredAgeErrorMessage)]
        [Range(PetAgeMin, PetAgeMax, ErrorMessage = PetAgeErrorMessage)]
        public int Age { get; set; }

        [Comment("Pet image URL")]
        public string? ImageUrl { get; set; }

        [Comment("Indicates if the pet is deleted")]
        public bool IsDeleted { get; set; } = false;

        [Comment("Notes for the pet")]
        [StringLength(NotesMaxLength, ErrorMessage = NotesMaxLengthErrorMessage)]
        public string? Notes { get; set; }

        [Comment("Foreign key to the owner of the pet")]
        public required string OwnerId { get; set; }
        public virtual IdentityUser Owner { get; set; } = null!;

        [Comment("Collection of appointments of the pet")]
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }

}
