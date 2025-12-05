namespace PetGroomingApp.Services.Core.Admin.Services
{
    using Data.Models;
    using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Web.ViewModels.Admin.Users;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<UserIndexViewModel>> GetUserManagementBoardDataAsync(string userId)
        {
            var allUsers = await this.userManager
                .Users
                .Where(u => u.Id.ToLower() != userId.ToLower())
                .ToListAsync();

            var users = new List<UserIndexViewModel>();
            foreach (var user in allUsers)
            {
                var roles = await this.userManager.GetRolesAsync(user);
                users.Add(new UserIndexViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? user.Email,
                    Email = user.Email ?? string.Empty,
                    Roles = roles
                });
            }

            return users;
        }

        public async Task<bool> AssignUserToRoleAsync(RoleSelectionInputModel inputModel)
        {
            ApplicationUser? user = await this.userManager
                .FindByIdAsync(inputModel.UserId);

            if (user == null)
            {
                throw new ArgumentException("User does not exist!");
            }

            bool roleExists = await this.roleManager.RoleExistsAsync(inputModel.Role);
            if (!roleExists)
            {
                throw new ArgumentException("Selected role is not a valid role!");
            }

            try
            {
                await this.userManager.AddToRoleAsync(user, inputModel.Role);

                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    "Unexpected error occurred while adding the user to role! Please try again later!",
                    innerException: e);
            }
        }

        public async Task<UserFormViewModel?> GetByIdAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var roles = await this.userManager.GetRolesAsync(user);
            return new UserFormViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SelectedRole = roles.FirstOrDefault()
            };
        }

        public async Task<UserFormViewModel?> GetForEditByIdAsync(string userId)
        {
            return await GetByIdAsync(userId);
        }

        public async Task<bool> CreateAsync(UserFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                throw new ArgumentException("Email is required!");

            if (string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Password is required!");

            var user = new ApplicationUser
            {
                UserName = model.UserName ?? model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await this.userManager.CreateAsync(user, model.Password!);
            
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Assign role if specified
            if (!string.IsNullOrWhiteSpace(model.SelectedRole))
            {
                var roleExists = await this.roleManager.RoleExistsAsync(model.SelectedRole);
                if (roleExists)
                {
                    await this.userManager.AddToRoleAsync(user, model.SelectedRole);
                }
            }

            return true;
        }

        public async Task<bool> EditAsync(string userId, UserFormViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User does not exist!");

            user.Email = model.Email;
            user.UserName = model.UserName ?? model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var result = await this.userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await this.userManager.ResetPasswordAsync(user, token, model.Password);
                if (!passwordResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", passwordResult.Errors.Select(e => e.Description)));
                }
            }

            // Update role if specified
            if (!string.IsNullOrWhiteSpace(model.SelectedRole))
            {
                var currentRoles = await this.userManager.GetRolesAsync(user);
                await this.userManager.RemoveFromRolesAsync(user, currentRoles);
                
                var roleExists = await this.roleManager.RoleExistsAsync(model.SelectedRole);
                if (roleExists)
                {
                    await this.userManager.AddToRoleAsync(user, model.SelectedRole);
                }
            }

            return true;
        }
    }
}
