namespace PetGroomingApp.Data.Repository
{
    using System;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class PetRepository : BaseRepository<Pet, Guid>, IPetRepository
    {
        public PetRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
