using ClientAppe.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientAppe.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        // Змінна для зберігання токену (знадобиться в 4-й лабі)
        public static string AccessToken { get; set; }

        public ApiService()
        {
            _httpClient = new HttpClient();

            _httpClient.BaseAddress = new Uri("https://localhost:44333/api/");
        }
        public async Task<List<RestaurantModel>> GetRestaurantsAsync()
        {
            await Task.Delay(300); // Імітуємо затримку інтернету
            return MockDataStorage.GetRestaurants();
        }
        public async Task<List<OrderModel>> GetOrdersAsync()
        {
            await Task.Delay(300); // Імітація мережі
            return MockDataStorage.GetOrders();
        }
        public async Task<UserModel> GetProfileAsync()
        {
            await Task.Delay(200);
            return MockDataStorage.GetUserProfile();
        }
        // ==========================================
        // РЕАЛЬНИЙ ЛОГІН
        // ==========================================
        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new { Login = email, Password = password };

                string json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal async Task<bool> RegisterAsync(string? username, string? email, string? password)
        {
            throw new NotImplementedException();
        }
    }
}