using ConsumirApiLibros.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using ApiLibros.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConsumirApiLibros.Services
{
    public class PrestamosService :IPrestamosService
    {
        //private readonly HttpClient _httpClient;
        private string _token = string.Empty;

        //public PrestamosService()
        //{
        //    _httpClient = new HttpClient();
        //    _httpClient.BaseAddress = new Uri("https://localhost:7069/");
        //}

        private readonly HttpClient _client;

        public PrestamosService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("ApiLibros");
        }


        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginData = new LoginRequest { Username = username, Password = password };
            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);

            // Asegúrate de usar la URL completa
            var response = await client.PostAsync("https://localhost:7069/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var auth = JsonSerializer.Deserialize<AuthResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _token = auth?.Token ?? string.Empty;

                return !string.IsNullOrEmpty(_token);
            }

            return false;
        }

        public async Task<List<Prestamos>> GetPrestamosAsync()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:7069/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.GetAsync("api/Prestamos/Todos");

            if (!response.IsSuccessStatusCode)
                return new List<Prestamos>();

            var content = await response.Content.ReadAsStringAsync();
            var prestamos = JsonSerializer.Deserialize<List<Prestamos>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return prestamos ?? new List<Prestamos>();
        }

        public async Task<bool>  InsertarPrestamos(Prestamos prestamo) 
        {
            var content = new StringContent(JsonSerializer.Serialize(prestamo),Encoding.UTF8,"application/json");

            var response = await _client.PostAsync("api/Prestamos", content);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseContent = await response.Content.ReadAsStringAsync();

            var prestamoInsertado =  JsonSerializer.Deserialize<Prestamos>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (prestamoInsertado == null)
            {
                return false;
            }

            return true;
        }
    }
}
