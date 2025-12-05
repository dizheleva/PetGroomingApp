namespace PetGroomingApp.Web.ViewModels
{
    public class SearchViewModel
    {
        public string? SearchTerm { get; set; }
        public string? FilterBy { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";
    }
}

