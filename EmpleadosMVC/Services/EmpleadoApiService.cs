using System.Net.Http.Headers;
using Empleados.Models;

namespace EmpleadosMVC.Services
{
    public class EmpleadoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly AuthService _authService;

        public EmpleadoApiService(HttpClient httpClient, IConfiguration configuration, AuthService authService)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiSettings:BaseUrl"] + "/api/empleados";
            _authService = authService;
        }

        private void SetAuthorizationHeader()
        {
            try
            {
                var token = _authService.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting auth header: {ex.Message}");
            }
        }
        public async Task<PaginatedResult<Empleado>> GetPagedEmpleadosAsync(int pageIndex, int pageSize, string searchTerm = null)
        {
            try
            {
                SetAuthorizationHeader();

                var queryString = $"?pageIndex={pageIndex}&pageSize={pageSize}";

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    queryString += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(_apiUrl + queryString);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Redirigir a la página de login
                    throw new UnauthorizedAccessException("No autorizado para acceder a este recurso. Por favor, inicie sesión.");
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PaginatedResult<Empleado>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<Empleado> GetEmpleadoByIdAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Empleado>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

        }

        public async Task<Empleado> CreateEmpleadoAsync(Empleado empleado)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync(_apiUrl, empleado);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Empleado>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

        }

        public async Task<Empleado> UpdateEmpleadoAsync(int id, Empleado empleado)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", empleado);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error en UpdateEmpleadoAsync: {response.StatusCode} - {errorContent}");
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Empleado>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception en UpdateEmpleadoAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteEmpleadoAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

        }
    }
}