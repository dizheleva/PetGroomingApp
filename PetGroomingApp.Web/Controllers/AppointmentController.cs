namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PetGroomingApp.Data.Models.Enums;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IGroomerService _groomerService;
        private readonly IPetService _petService;
        private readonly IServiceService _serviceService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            IAppointmentService appointmentService, 
            IGroomerService groomerService, 
            IPetService petService, 
            IServiceService serviceService,
            ILogger<AppointmentController> logger)
        {
            _appointmentService = appointmentService;
            _groomerService = groomerService;
            _petService = petService;
            _serviceService = serviceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null, string? statusFilter = null)
        {
            const int pageSize = 6;
            IEnumerable<AppointmentListViewModel> appointments = await _appointmentService.GetByUserAsync(GetUserId());

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                appointments = appointments.Where(a => 
                    a.PetName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Id.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                appointments = appointments.Where(a => a.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Pagination
            var totalItems = appointments.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedAppointments = Infrastructure.Helpers.PaginationHelper.Paginate(appointments, page, pageSize);

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
                AppointmentDetailsViewModel? appointment = null;
                
                if (User.Identity?.IsAuthenticated == true)
                {
                    appointment = await _appointmentService.GetDetailsAsync(id, GetUserId());
                }

                if (appointment == null)
                {
                    return NotFound();
                }

                return View(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment details for ID: {AppointmentId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the appointment details.");
                return View(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {     
            var model = new AppointmentUserFormViewModel();
            await PopulateUserSelectListsAsync(model);

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentUserFormViewModel model)
        {            
            if(User.Identity?.IsAuthenticated != true)
            {
                ModelState.AddModelError(string.Empty, "You must be logged in to create an appointment.");
                return RedirectToAction(nameof(Index));
            }            

            if (model.SelectedServiceIds == null || model.SelectedServiceIds.Count == 0)
            {
                ModelState.AddModelError("", "You must select at least one service.");
                await PopulateUserSelectListsAsync(model);
                return View(model);
            }

            await PopulateUserSelectListsAsync(model);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed for appointment creation");
                return View(model);
            }

            try
            {
                await _appointmentService.CreateAsync(model, GetUserId());
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating appointment");
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateUserSelectListsAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating appointment");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the appointment. Please try again.");
                await PopulateUserSelectListsAsync(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var model = await _appointmentService.GetForEditByIdAsync(id, GetUserId());

                if (model == null)
                {
                    return NotFound();
                }

                await PopulateUserSelectListsAsync(model);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment for editing. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the appointment for editing.";
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AppointmentUserFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateUserSelectListsAsync(model);

                return View(model);
            }

            try
            {
                bool editSuccess = await _appointmentService.EditAsync(id, model, GetUserId());
                if (!editSuccess)
                {
                    TempData["ErrorMessage"] = "Failed to update appointment.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Appointment updated successfully.";
                return this.RedirectToAction(nameof(Details), new { id = id });
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this appointment.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await PopulateUserSelectListsAsync(model);
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while editing appointment. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = "An error occurred while editing the appointment. Please try again.";
                await PopulateUserSelectListsAsync(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(string id)
        {
            try
            {
                var appointment = await _appointmentService.GetDetailsAsync(id, GetUserId());
                if (appointment == null)
                {
                    return NotFound();
                }
                return View(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment for cancellation. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the appointment.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(string id)
        {
            try
            {
                var success = await _appointmentService.CancelAsync(id, GetUserId());
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Appointment could not be canceled.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error canceling appointment. ID: {AppointmentId}", id);
                TempData["ErrorMessage"] = "An error occurred while canceling the appointment. Please try again.";
                return this.RedirectToAction(nameof(Index));
            }

        }

        private async Task PopulateUserSelectListsAsync(AppointmentUserFormViewModel model)
        {
            model.Groomers = (await _groomerService.GetAllAsync()).Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToList();

            model.Pets = (await _petService.GetPetsByUserAsync(GetUserId())).Select(p => new SelectListItem
            {
                Value = p?.Id.ToString() ?? null,
                Text = p?.Name ?? "No Pets Available"
            }).ToList();

            model.Services = (await _serviceService.GetAllAsync()).Select(s => new SelectListItem
            {
                Value = s.Id,
                Text = s.Name,
            }).ToList();
        }

    }
}
