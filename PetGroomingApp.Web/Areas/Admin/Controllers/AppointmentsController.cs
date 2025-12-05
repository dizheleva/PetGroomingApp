namespace PetGroomingApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Models.Enums;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public class AppointmentsController : BaseAdminController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IGroomerService _groomerService;
        private readonly IPetService _petService;
        private readonly IServiceService _serviceService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IGroomerService groomerService,
            IPetService petService,
            IServiceService serviceService,
            UserManager<ApplicationUser> userManager,
            ILogger<AppointmentsController> logger)
        {
            _appointmentService = appointmentService;
            _groomerService = groomerService;
            _petService = petService;
            _serviceService = serviceService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AppointmentUserFormViewModel();
            await PopulateAdminSelectListsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentUserFormViewModel model)
        {
            if (model.SelectedServiceIds == null || model.SelectedServiceIds.Count == 0)
            {
                ModelState.AddModelError("", "You must select at least one service.");
                await PopulateAdminSelectListsAsync(model);
                return View(model);
            }

            // Validate pet: either SelectedPetId or PetName must be provided
            if (!model.SelectedPetId.HasValue && string.IsNullOrWhiteSpace(model.PetName))
            {
                ModelState.AddModelError("PetName", "Either select a pet or enter a pet name.");
            }

            await PopulateAdminSelectListsAsync(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Admin can create appointments without user (for unregistered customers)
                var userId = string.IsNullOrWhiteSpace(model.UserId) ? null : model.UserId;
                await _appointmentService.CreateAsync(model, userId);
                TempData["SuccessMessage"] = "Appointment created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating appointment as admin");
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateAdminSelectListsAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating appointment as admin");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the appointment. Please try again.");
                await PopulateAdminSelectListsAsync(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null, string? statusFilter = null)
        {
            const int pageSize = 6;
            var allAppointments = await _appointmentService.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allAppointments = allAppointments.Where(a => 
                    a.PetName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.OwnerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.GroomerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Id.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                allAppointments = allAppointments.Where(a => a.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Pagination
            var totalItems = allAppointments.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedAppointments = Infrastructure.Helpers.PaginationHelper.Paginate(allAppointments, page, pageSize);

            ViewBag.Pagination = pagination;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = statusFilter;

            return View(paginatedAppointments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                // Admin can see all appointments - use empty userId
                var appointmentDetails = await _appointmentService.GetDetailsAsync(id, string.Empty);
                if (appointmentDetails == null)
                {
                    TempData["ErrorMessage"] = "Appointment not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(appointmentDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment details as admin. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the appointment details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                // Get appointment for editing (no userId check for admin)
                var model = await _appointmentService.GetForEditByIdAsync(id, string.Empty);

                if (model == null)
                {
                    TempData["ErrorMessage"] = "Appointment not found.";
                    return RedirectToAction(nameof(Index));
                }

                await PopulateAdminSelectListsAsync(model);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment for editing as admin. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the appointment for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AppointmentUserFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAdminSelectListsAsync(model);
                return View(model);
            }

            try
            {
                bool editSuccess = await _appointmentService.EditAsync(id, model, string.Empty);
                if (!editSuccess)
                {
                    TempData["ErrorMessage"] = "Failed to update appointment.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Appointment updated successfully.";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while editing appointment as admin. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = ex.Message;
                await PopulateAdminSelectListsAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while editing appointment as admin. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = "An error occurred while editing the appointment. Please try again.";
                await PopulateAdminSelectListsAsync(model);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(string id, string status)
        {
            try
            {
                var model = await _appointmentService.GetForEditByIdAsync(id, string.Empty);
                if (model == null)
                {
                    TempData["ErrorMessage"] = "Appointment not found.";
                    return RedirectToAction(nameof(Index));
                }

                model.Status = status;
                bool success = await _appointmentService.EditAsync(id, model, string.Empty);
                
                if (success)
                {
                    TempData["SuccessMessage"] = $"Appointment status changed to {status}.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to change appointment status.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing appointment status as admin. ID: {AppointmentId}, Status: {Status}", id, status);
                TempData["ErrorMessage"] = "An error occurred while changing the appointment status. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task PopulateAdminSelectListsAsync(AppointmentUserFormViewModel model)
        {
            model.Groomers = (await _groomerService.GetAllAsync()).Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToList();

            // Get all users from UserManager instead of from appointments
            var allUsers = await _userManager.Users.ToListAsync();
            model.Users = allUsers
                .Where(u => u != null && !string.IsNullOrWhiteSpace(u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName ?? u.Email ?? "Unknown User",
                    Selected = u.Id == model.UserId
                }).ToList();
            // Add option for unregistered customers
            model.Users.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Unregistered Customer (No User)",
                Selected = string.IsNullOrWhiteSpace(model.UserId)
            });

            // Only load pets if a user is selected
            if (!string.IsNullOrWhiteSpace(model.UserId))
            {
                var userPets = await _petService.GetPetsByUserAsync(model.UserId);
                model.Pets = userPets
                    .Where(p => p != null)
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id,
                        Text = p.Name,
                        Selected = p.Id == model.SelectedPetId?.ToString()
                    }).ToList();
            }
            else
            {
                model.Pets = new List<SelectListItem>();
            }

            model.Services = (await _serviceService.GetAllAsync()).Select(s => new SelectListItem
            {
                Value = s.Id,
                Text = s.Name,
            }).ToList();

            model.Status = model.Status ?? "Pending";
        }
    }
}

