namespace PetGroomingApp.Data.Seeding.Dtos
{
    public class AppointmentInputModel
    {
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public Guid PetId { get; set; }
        public required string UserId { get; set; }
        public Guid? GroomerId { get; set; }
        public int Status { get; set; }
    }

}
