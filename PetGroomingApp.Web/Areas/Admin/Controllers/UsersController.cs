namespace PetGroomingApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Identity;

    using PetGroomingApp.Services.Core.Admin.Interfaces;
    using PetGroomingApp.Web.ViewModels.Admin.Users;

    using static GCommon.ApplicationConstants;

    public class UsersController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUserService userService, RoleManager<IdentityRole> roleManager, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null)
        {
            const int pageSize = 6;
            IEnumerable<UserIndexViewModel> allUsers = await this.userService
                .GetUserManagementBoardDataAsync(this.GetUserId()!);

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allUsers = allUsers.Where(u => 
                    (u.UserName != null && u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Roles.Any(r => r.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Pagination
            var totalItems = allUsers.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedUsers = Infrastructure.Helpers.PaginationHelper.Paginate(allUsers, page, pageSize);

            ViewBag.Pagination = pagination;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedUsers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var user = await this.userService.GetByIdAsync(id);
                if (user == null)
                {
                    TempData[ErrorMessageKey] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving user details as admin. ID: {UserId}", id);
                TempData[ErrorMessageKey] = "An error occurred while retrieving the user details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new UserFormViewModel();
            PopulateRolesSelectList(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateRolesSelectList(model);
                return View(model);
            }

            try
            {
                await this.userService.CreateAsync(model);
                TempData[SuccessMessageKey] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating user as admin");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the user. Please try again.");
                PopulateRolesSelectList(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var model = await this.userService.GetForEditByIdAsync(id);
                if (model == null)
                {
                    TempData[ErrorMessageKey] = "User not found for editing.";
                    return RedirectToAction(nameof(Index));
                }

                PopulateRolesSelectList(model);
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving user for editing as admin. ID: {UserId}", id);
                TempData[ErrorMessageKey] = "An error occurred while retrieving the user for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserFormViewModel model)
        {
            // Custom validation for password - if password is provided, confirm password must match
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Please confirm the password.");
                }
                else if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "The password and confirmation password do not match.");
                }
            }

            // Remove password validation errors if password is not provided (for edit scenario)
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));
            }

            if (!ModelState.IsValid)
            {
                PopulateRolesSelectList(model);
                return View(model);
            }

            try
            {
                bool editSuccess = await this.userService.EditAsync(id, model);
                if (!editSuccess)
                {
                    TempData[ErrorMessageKey] = "Failed to edit user.";
                    return RedirectToAction(nameof(Index));
                }
                TempData[SuccessMessageKey] = "User updated successfully!";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error editing user as admin. ID: {UserId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while editing the user. Please try again.");
                PopulateRolesSelectList(model);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(RoleSelectionInputModel inputModel)
        {
            try
            {
                await this.userService
                    .AssignUserToRoleAsync(inputModel);
                TempData[SuccessMessageKey] = "User assigned to role successfully!";

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] = e.Message;

                return this.RedirectToAction(nameof(Index));
            }
        }

        private void PopulateRolesSelectList(UserFormViewModel model)
        {
            model.Roles = this.roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name,
                    Selected = r.Name == model.SelectedRole
                })
                .ToList();
        }
    }
}
