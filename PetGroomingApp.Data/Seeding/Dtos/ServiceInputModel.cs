namespace PetGroomingApp.Data.Seeding.Dtos
{
    public class ServiceInputModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Duration { get; set; }
        public decimal Price { get; set; }
    }
}
