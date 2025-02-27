using Empleados.Models;
using EmpleadosMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpleadosMVC.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoApiService _empleadoApiService;
        private readonly ILogger<EmpleadosController> _logger;
        private readonly AuthService _authService;

        public EmpleadosController(EmpleadoApiService empleadoApiService, ILogger<EmpleadosController> logger, AuthService authService)
        {
            _empleadoApiService = empleadoApiService;
            _logger = logger;
            _authService = authService;
        }

        // GET: Empleados
        public async Task<IActionResult> Index(string searchTerm, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                if (User.Identity.IsAuthenticated && !_authService.IsAuthenticated())
                {
                    // Obtiene token para el usuario de Identity
                    await _authService.GetTokenForIdentityUserAsync(User.Identity.Name);
                }
                ViewBag.CurrentSearchTerm = searchTerm;

                var result = await _empleadoApiService.GetPagedEmpleadosAsync(pageIndex, pageSize, searchTerm);
                return View(result);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Index", "Empleados") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de empleados");
                TempData["ErrorMessage"] = "No se pudo obtener la lista de empleados. Por favor, intente de nuevo más tarde.";
                return View(new PaginatedResult<Empleado>
                {
                    Items = new List<Empleado>(),
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = 0
                });
            }
        }

        // GET: Empleados/Details/1
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var empleado = await _empleadoApiService.GetEmpleadoByIdAsync(id);
                return View(empleado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener los detalles del empleado con ID {id}");
                TempData["ErrorMessage"] = "No se pudieron obtener los detalles del empleado.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoApiService.CreateEmpleadoAsync(empleado);
                    TempData["SuccessMessage"] = "Empleado creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear el empleado");
                    ModelState.AddModelError("", "No se pudo crear el empleado. Por favor, verifique los datos e intente nuevamente.");
                }
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var empleado = await _empleadoApiService.GetEmpleadoByIdAsync(id);
                return View(empleado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el empleado con ID {id} para editar");
                TempData["ErrorMessage"] = "No se pudo obtener la información del empleado para editar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Empleados/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                TempData["ErrorMessage"] = "ID de empleado no válido.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoApiService.UpdateEmpleadoAsync(id, empleado);
                    TempData["SuccessMessage"] = "Empleado actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al actualizar el empleado con ID {id}");
                    ModelState.AddModelError("", "No se pudo actualizar el empleado. Por favor, verifique los datos e intente nuevamente.");
                }
            }
            return View(empleado);
        }

        // GET: Empleados/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var empleado = await _empleadoApiService.GetEmpleadoByIdAsync(id);
                return View(empleado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el empleado con ID {id} para eliminar");
                TempData["ErrorMessage"] = "No se pudo obtener la información del empleado para eliminar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Empleados/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _empleadoApiService.DeleteEmpleadoAsync(id);
                TempData["SuccessMessage"] = "Empleado eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el empleado con ID {id}");
                TempData["ErrorMessage"] = "No se pudo eliminar el empleado.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}