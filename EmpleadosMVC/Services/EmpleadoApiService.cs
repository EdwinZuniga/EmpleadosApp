using Empleados.Models;


namespace EmpleadosMVC.Services
{
    public class EmpleadoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public EmpleadoApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiSettings:BaseUrl"] + "/api/empleados";
        }

        public async Task<IEnumerable<Empleado>> GetAllEmpleadosAsync()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Empleado>>();
        }

        public async Task<PaginatedResult<Empleado>> GetPagedEmpleadosAsync(int pageIndex, int pageSize, string searchTerm = null)
        {
            var queryString = $"?pageIndex={pageIndex}&pageSize={pageSize}";
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                queryString += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
            }

            var response = await _httpClient.GetAsync(_apiUrl + queryString);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PaginatedResult<Empleado>>();
        }

        public async Task<Empleado> GetEmpleadoByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Empleado>();
        }

        public async Task<Empleado> CreateEmpleadoAsync(Empleado empleado)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, empleado);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Empleado>();
        }

        public async Task<Empleado> UpdateEmpleadoAsync(int id, Empleado empleado)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", empleado);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Empleado>();
        }

        public async Task DeleteEmpleadoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}