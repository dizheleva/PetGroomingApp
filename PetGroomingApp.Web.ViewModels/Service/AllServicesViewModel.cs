namespace PetGroomingApp.Web.ViewModels.Service
{
    public class AllServicesViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Duration { get; set; } // Duration in minutes
    }
}
