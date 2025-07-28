namespace PetGroomingApp.Services.Core.Interfaces
{
    public interface IService<TType>
    where TType : class
    {
        Task<TType?> GetByStringId(string? id);
    }
}
