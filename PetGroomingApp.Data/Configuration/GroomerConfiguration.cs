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

            builder.HasData(SeedGroomers());
        }

        private static List<Groomer> SeedGroomers()
        {
            return new List<Groomer>()
            {
                new Groomer
                {
                    Id = Guid.Parse("5ae6c761-1363-4a23-9965-171c70f935de"),
                    FirstName = "Lara",
                    LastName = "Smith",
                    JobTitle = "Groomer, Manager",
                    ImageUrl = "img/team/1.png",
                    PhoneNumber = "555-5678",
                    Description = "Experienced groomer with a passion for animals.",
                },
                new Groomer
                {
                    Id = Guid.Parse("f4c3e429-0e36-47af-99a2-0c7581a7fc67"),
                    FirstName = "Tom",
                    LastName = "Brown",
                    JobTitle = "Groomer Assistant",
                    ImageUrl = "img/team/2.png",
                    PhoneNumber = "555-1234",
                    Description = "Loves grooming and caring for pets.",
                },
                new Groomer
                {
                    Id = Guid.Parse("b2c1d3e4-5f6a-7b8c-9d0e-1f2a3b4c5d6e"),
                    FirstName = "Sara",
                    LastName = "Croft",
                    JobTitle = "Senior Groomer",
                    ImageUrl = "img/team/3.png",
                    PhoneNumber = "555-8765",
                    Description = "Expert in grooming all breeds of dogs and cats.",
                }
            };
        }
    }
}
