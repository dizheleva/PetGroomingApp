namespace PetGroomingApp.Data.Repository.Interfaces
{
    using PetGroomingApp.Data.Models;

    public interface IPetRepository : IRepository<Pet, Guid>
    {
    }
}
