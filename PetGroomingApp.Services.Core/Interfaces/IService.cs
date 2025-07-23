namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Service;

    public interface IService<TType>
    where TType : class
    {
        Task<TType?> GetByStringId(string? id);
    }
}
