namespace PetGroomingApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PetGroomingApp.Data.Models;

    public class UserServiceConfiguration : IEntityTypeConfiguration<UserService>
    {
        public void Configure(EntityTypeBuilder<UserService> builder)
        {
            builder.HasKey(us => new { us.UserId, us.ServiceId });

            builder
                .HasOne(us => us.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(us => us.Service)
                .WithMany()
                .HasForeignKey(us => us.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
