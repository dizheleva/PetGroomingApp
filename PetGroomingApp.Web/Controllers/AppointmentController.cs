namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IGroomerService _groomerService;
        private readonly IPetService _petService;
        private readonly IServiceService _serviceService;

        public AppointmentController(IAppointmentService appointmentService, IGroomerService groomerService, IPetService petService, IServiceService serviceService)
        {
            _appointmentService = appointmentService;
            _groomerService = groomerService;
            _petService = petService;
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Manager"))
            {
                return View(await _appointmentService.GetAllAsync());
            }

            var appointments = await _appointmentService.GetByUserAsync(GetUserId());

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {            
            try
            {
                AppointmentDetailsViewModel? appointment = null;
                
                if (User.IsInRole("Manager"))
                {
                    appointment = await _appointmentService.GetDetailsAsManagerAsync(id);
                }
                else if (User.Identity?.IsAuthenticated == true)
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
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the appointment details: {ex.Message}");
                return View(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AppointmentFormViewModel
            {
                Groomers = (await _groomerService.GetAllAsync()).Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                }),

                Pets = (await _petService.GetPetsByUserAsync(GetUserId())).Select(p => new SelectListItem
                {
                    Value = p?.Id.ToString() ?? null,
                    Text = p?.Name ?? "No Pets Available"
                }),

                Services = (await _serviceService.GetAllAsync()).Select(s => new ViewModels.Service.AllServicesViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentFormViewModel model)
        {            
            if(User.Identity?.IsAuthenticated != true)
            {
                ModelState.AddModelError(string.Empty, "You must be logged in to create an appointment.");
                await PopulateSelectListsAsync(model);
                return View(model);
            }            

            if (model.SelectedServiceIds == null || model.SelectedServiceIds.Count == 0)
            {
                ModelState.AddModelError("", "You must select at least one service.");
                return View(model);
            }

            Console.WriteLine("Creating appointment for user: " + GetUserId());
            Console.WriteLine("Selected services: " + string.Join(", ", model.SelectedServiceIds));


            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"  Error: {err.ErrorMessage}");
                    }
                }
                await PopulateSelectListsAsync(model);
                return View(model);
            }
            Console.WriteLine("Creating appointment for user: " + GetUserId());
            Console.WriteLine("Selected services: " + string.Join(", ", model.SelectedServiceIds));

            try
            {
                var ownerId = User.IsInRole("Manager") ? null : GetUserId();
                await _appointmentService.CreateAsync(model, ownerId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the appointment: {ex.Message}");
                await PopulateSelectListsAsync(model);
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

                await PopulateSelectListsAsync(model);

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the appointment for editing: {ex.Message}");

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AppointmentFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync(model);

                return View(model);
            }
            try
            {
                bool editSuccess = await _appointmentService.EditAsync(id, model, GetUserId());
                if (!editSuccess)
                {
                    return NotFound();
                }

                return this.RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while editing the appointment: {e.Message}");

                await PopulateSelectListsAsync(model);

                return this.RedirectToAction(nameof(Index));
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
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the appointment for cancelation: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
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
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while canceling the appointment: {e.Message}");
                return this.RedirectToAction(nameof(Index));
            }

        }

        private async Task PopulateSelectListsAsync(AppointmentFormViewModel model)
        {
            model.Groomers = (await _groomerService.GetAllAsync()).Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            });

            model.Pets = (await _petService.GetPetsByUserAsync(GetUserId())).Select(p => new SelectListItem
            {
                Value = p?.Id.ToString() ?? null,
                Text = p?.Name ?? "No Pets Available"
            });

            model.Services = (await _serviceService.GetAllAsync()).Select(s => new ViewModels.Service.AllServicesViewModel
            {
                Id = s.Id,
                Name = s.Name,
            });
        }
    }
}
