namespace PetGroomingApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasOne(m => m.User)
                .WithOne()
                .HasForeignKey<Manager>(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(m => new { m.UserId })
                .IsUnique();

            builder
                .HasQueryFilter(m => m.IsDeleted == false);
        }
    }
}
