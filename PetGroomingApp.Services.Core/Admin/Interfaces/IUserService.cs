namespace PetGroomingApp.Services.Core.Admin.Interfaces
{
    using Web.ViewModels.Admin.Users;

    public interface IUserService
    {
        Task<IEnumerable<UserIndexViewModel>> GetUserManagementBoardDataAsync(string userId);

        Task<IEnumerable<string>> GetManagerEmailsAsync();

        Task<bool> AssignUserToRoleAsync(RoleSelectionInputModel inputModel);
    }
}
