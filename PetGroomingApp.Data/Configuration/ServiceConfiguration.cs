namespace PetGroomingApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Seeding;

    using static PetGroomingApp.Data.Common.EntityConstansts.ServiceConstants;

    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(ServiceNameMaxLength);

            builder.Property(s => s.Description)
                .IsRequired(false)
                .HasMaxLength(ServiceDescriptionMaxLength);

            builder.Property(s => s.Duration)
                .IsRequired();

            builder.Property(s => s.Price)
                .HasColumnType(ServicePriceSqlColumnType)
                .IsRequired();

            builder.Property(s => s.IsDeleted)
                .HasDefaultValue(false);

            builder.HasIndex(s => s.Name);
            builder.HasIndex(s => s.Price);

            builder.ToTable(t => t.HasComment("Service entity representing available grooming services"));

            builder.HasData(DbSeeder.SeedServices());
        }
    }
}
