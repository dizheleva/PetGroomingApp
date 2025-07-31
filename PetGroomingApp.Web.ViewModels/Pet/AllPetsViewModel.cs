namespace PetGroomingApp.Web.ViewModels.Pet
{
    public class AllPetsViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Type { get; set; }

        public required string Breed { get; set; }

        public required string ImageUrl { get; set; }
    }
}
