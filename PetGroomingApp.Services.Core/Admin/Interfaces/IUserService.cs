namespace PetGroomingApp.Services.Core.Admin.Interfaces
{
    using Web.ViewModels.Admin.Users;

    public interface IUserService
    {
        Task<IEnumerable<UserIndexViewModel>> GetUserManagementBoardDataAsync(string userId);

        Task<bool> AssignUserToRoleAsync(RoleSelectionInputModel inputModel);

        Task<UserFormViewModel?> GetForEditByIdAsync(string userId);

        Task<bool> CreateAsync(UserFormViewModel model);

        Task<bool> EditAsync(string userId, UserFormViewModel model);

        Task<UserFormViewModel?> GetByIdAsync(string userId);
    }
}
