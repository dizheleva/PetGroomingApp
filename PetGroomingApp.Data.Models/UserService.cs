namespace PetGroomingApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class UserService
    {
        public string UserId { get; set; } = null!; 
        public virtual IdentityUser User { get; set; } = null!;
        public Guid ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;
    }
}
