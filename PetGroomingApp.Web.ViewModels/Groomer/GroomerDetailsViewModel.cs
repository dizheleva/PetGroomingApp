namespace PetGroomingApp.Web.ViewModels.Groomer
{
    public class GroomerDetailsViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string JobTitle { get; set; }

        public required string ImageUrl { get; set; }

        public required string PhoneNumber { get; set; }

        public string? Description { get; set; }
    }
}
