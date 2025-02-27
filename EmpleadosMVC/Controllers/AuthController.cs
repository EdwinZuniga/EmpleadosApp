using Microsoft.AspNetCore.Mvc;
using EmpleadosMVC.Services;
using EmpleadosMVC.Models;

namespace EmpleadosMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        // En EmpleadosMVC/Controllers/AuthController.cs
        public async Task<IActionResult> GetApiToken()
        {
            // Verifica si el usuario está autenticado en Identity
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name; // En Identity, Name suele ser el email

                try
                {
                    var response = await _authService.GetTokenForIdentityUserAsync(userEmail);
                    if (response)
                    {
                        return RedirectToAction("Index", "Empleados");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al obtener token API: {ex.Message}";
                }
            }

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}