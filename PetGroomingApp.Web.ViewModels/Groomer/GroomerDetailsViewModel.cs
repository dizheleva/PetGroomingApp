namespace PetGroomingApp.Web.ViewModels.Groomer
{
    public class GroomerDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string JobTitle { get; set; }

        public required string ImageUrl { get; set; }

        public required string PhoneNumber { get; set; }

        public string? Description { get; set; }
    }
}
