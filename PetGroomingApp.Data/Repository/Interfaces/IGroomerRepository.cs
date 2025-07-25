namespace PetGroomingApp.Data.Repository.Interfaces
{
    using PetGroomingApp.Data.Models;

    public interface IGroomerRepository : IRepository<Groomer>
    {
        Task<Groomer> GetTopRatedGroomersAsync(int count);
    }
}
