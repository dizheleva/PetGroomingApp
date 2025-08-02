namespace PetGroomingApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PetGroomingApp.Data.Models;

    using static PetGroomingApp.Data.Common.EntityConstansts.AppointmentConstants;

    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedOn)
                .HasComment("Timestamp of when the appointment was created")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(a => a.AppointmentTime)
                .IsRequired();

            builder.Property(a => a.Notes)
                .HasMaxLength(NotesMaxLength)
                .IsRequired(false);

            builder.HasOne(a => a.Pet)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.User)
                .WithMany(u => u.Appointments) 
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Groomer)
                .WithMany(g => g.Appointments)
                .HasForeignKey(a => a.GroomerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(a => a.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.HasMany(a => a.AppointmentServices)
                .WithOne(asg => asg.Appointment)
                .HasForeignKey(asg => asg.AppointmentId);

            builder.HasIndex(a => a.AppointmentTime).HasDatabaseName("Appointment_Time");
            builder.HasIndex(a => a.PetId);
            builder.HasIndex(a => a.GroomerId);
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.Status);
        }
    }
}
