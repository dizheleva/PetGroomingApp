namespace PetGroomingApp.Web.ViewModels.Service
{
    public class AllServicesIndexViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string ImageUrl { get; set; }   

        public string? Description { get; set; }

        public required string Duration { get; set; }

        public decimal Price { get; set; }
    }
}
