namespace PetGroomingApp.Services.Core.Interfaces
{
    public interface IService<TType> where TType : class
    {
        Task<bool> SoftDeleteAsync(string? id);
        Task<bool> HardDeleteAsync(string? id);
        Task<bool> ExistsAsync(string? id);
    }
}
