namespace PetGroomingApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PetGroomingApp.Data.Models;

    using static PetGroomingApp.Data.Common.EntityConstansts.PetConstants;

    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(PetNameMaxLength);

            builder.Property(p => p.Type)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.Size)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.Breed)
                .IsRequired()
                .HasMaxLength(BreedMaxLength);

            builder.Property(p => p.Age)
                .IsRequired();
                
            builder.Property(p => p.Notes)
                .IsRequired(false)
                .HasMaxLength(NotesMaxLength);

            builder
                .Property(p => p.ImageUrl)
                .HasMaxLength(ImageUrlMaxLength)
                .IsRequired(false);

            builder
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(p => p.Owner)
                .WithMany(o => o.Pets)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasQueryFilter(p => p.IsDeleted == false);

            builder.HasIndex(p => p.OwnerId);
            builder.HasIndex(p => p.Type);
            builder.HasIndex(p => p.Size);
        }
    }
}
