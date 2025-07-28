namespace PetGroomingApp.Web.ViewModels.Pet
{
    using PetGroomingApp.Data.Models.Enums;

    public class PetDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public required string Name { get; set; }

        public required string ImageUrl { get; set; }

        public required PetType Type { get; set; }

        public required string Breed { get; set; }

        public required PetSize Size { get; set; }

        public PetGender Gender { get; set; }

        public int Age { get; set; }

        public string? Notes { get; set; }
    }
}
