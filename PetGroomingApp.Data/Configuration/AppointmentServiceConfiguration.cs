namespace PetGroomingApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PetGroomingApp.Data.Models;

    public class AppointmentServiceConfiguration : IEntityTypeConfiguration<AppointmentService>
    {
        public void Configure(EntityTypeBuilder<AppointmentService> builder)
        {
            builder.HasKey(aps => new { aps.AppointmentId, aps.ServiceId});

            builder.HasOne(aps => aps.Appointment)
                .WithMany(a => a.AppointmentServices)
                .HasForeignKey(aps => aps.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(aps => aps.Service)
                .WithMany(s => s.AppointmentServices)
                .HasForeignKey(aps => aps.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(aps => aps.AppointmentId);
            builder.HasIndex(aps => aps.ServiceId);

            builder.ToTable(t => t.HasComment("Join table mapping appointments to services"));
        }
    }
}
