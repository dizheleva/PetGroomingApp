namespace PetGroomingApp.Web.ViewModels.Admin.Users
{
    public class UserIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string? UserName { get; set; }

        public string Email { get; set; } = null!;

        public IEnumerable<string> Roles { get; set; } = null!;
    }
}
