namespace PetGroomingApp.Data.Repository.Interfaces
{
    using PetGroomingApp.Data.Models;

    public interface IGroomerRepository : IRepository<Groomer, Guid>
    {
        Task<List<DateTime>?> GetGroomerAvailableTimes(Guid groomerId, int durationMinutes);
        Task<List<Groomer>?> GetAllAvailableAtAsync(DateTime time, int durationMinutes);
    }
}
