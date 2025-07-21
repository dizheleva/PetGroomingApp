namespace PetGroomingApp.Web.ViewModels.Service
{
    using System.ComponentModel.DataAnnotations;

    public class ServiceDetailsViewModel
    {
        public string Id { get; set; }
        public required string Name { get; set; }
                
        public required string ImageUrl { get; set; }

        public string? Description { get; set; }

        public required string Duration { get; set; }

        public decimal Price { get; set; }
    }
}
