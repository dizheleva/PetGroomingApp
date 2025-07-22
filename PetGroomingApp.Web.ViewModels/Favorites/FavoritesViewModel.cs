namespace PetGroomingApp.Web.ViewModels.Favorites
{
    public class FavoritesViewModel
    {
        public required string ServiceId { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
