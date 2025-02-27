using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace EmpleadosMVC.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string TokenKey = "JwtToken";

        public AuthService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                Console.WriteLine($"Intentando login para {email}");

                var loginData = new { Email = email, Password = password };
                var response = await _httpClient.PostAsJsonAsync(_configuration["ApiSettings:BaseUrl"] + "/api/auth/login", loginData);

                Console.WriteLine($"Respuesta de API login: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error de login: {errorContent}");
                    return false;
                }

                var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>();
                string token = responseContent.GetProperty("token").GetString();

                Console.WriteLine($"Token obtenido: {(token?.Length > 0 ? $"({token.Substring(0, 10)}...)" : "VACÍO")}");

                // Guardar token en sesión
                _httpContextAccessor.HttpContext.Session.SetString(TokenKey, token);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en LoginAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> GetTokenForIdentityUserAsync(string email)
        {
            var loginData = new { Email = email };
            var response = await _httpClient.PostAsJsonAsync(_configuration["ApiSettings:BaseUrl"] + "/api/auth/identity-login", loginData);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>();
            string token = responseContent.GetProperty("token").GetString();

            // Guardar token en sesión
            _httpContextAccessor.HttpContext.Session.SetString(TokenKey, token);

            return true;
        }
        public string GetToken()
        {
            try
            {
                return _httpContextAccessor.HttpContext?.Session?.GetString(TokenKey);
            }
            catch
            {
                return null;
            }
        }

        public bool IsAuthenticated()
        {
            try
            {
                return !string.IsNullOrEmpty(GetToken());
            }
            catch
            {
                return false;
            }
        }
    }
}