namespace PetGroomingApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class UserService
    {
        public required string UserId { get; set; }
        public virtual required IdentityUser User { get; set; }
        public Guid ServiceId { get; set; }
        public virtual required Service Service { get; set; }
    }
}
