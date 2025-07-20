namespace PetGroomingApp.Data.Seeding.Dtos
{
    public class PetInputModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public int Type { get; set; }
        public required string Breed { get; set; }
        public int Size { get; set; }
        public int Age { get; set; }
        public string? ImageUrl { get; set; }
        public string? Notes { get; set; }
        public required string OwnerId { get; set; }
    }

}
