namespace PetGroomingApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PetGroomingApp.Data.Models;

    using static PetGroomingApp.Data.Common.EntityConstansts.GroomerConstants;
    public class GroomerConfiguration : IEntityTypeConfiguration<Groomer>
    {
        public void Configure(EntityTypeBuilder<Groomer> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.FirstName)
                .IsRequired()
                .HasMaxLength(FirstNameMaxLength);

            builder.Property(g => g.LastName)
                .IsRequired()
                .HasMaxLength(LastNameMaxLength);

            builder.Property(g => g.PhoneNumber)
                .HasMaxLength(PhoneNumberMaxLength);

            builder.Property(g => g.Description)
                .HasMaxLength(DescriptionMaxLength);

            builder.HasIndex(g => new { g.FirstName, g.LastName });

            builder.HasQueryFilter(g => g.IsDeleted ==  false);
        }
    }
}
