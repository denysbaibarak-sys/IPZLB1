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
        private static readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:44333/api/")
        };
        // Змінна для зберігання глобального профілю користувача після логіну
        public static UserModel CurrentUser { get; set; }

        public ApiService(){}

        public async Task<List<RestaurantModel>> GetRestaurantsAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("restaurants");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Налаштування для ігнорування регістру букв у JSON
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    return JsonSerializer.Deserialize<List<RestaurantModel>>(jsonResponse, options) ?? new List<RestaurantModel>();
                }
                return new List<RestaurantModel>();
            }
            catch (Exception)
            {
                return new List<RestaurantModel>(); // Повертаємо пустий список при помилці
            }
        }

        public async Task<bool> CreateOrderAsync(OrderModel order)
        {
            try
            {
                string json = JsonSerializer.Serialize(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Відправляємо замовлення на контролер
                HttpResponseMessage response = await _httpClient.PostAsync("orders/create", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<OrderModel>> GetOrdersAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("orders");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<List<OrderModel>>(jsonResponse, options) ?? new List<OrderModel>();
                }
                return new List<OrderModel>();
            }
            catch (Exception)
            {
                return new List<OrderModel>();
            }
        }

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
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    CurrentUser = JsonSerializer.Deserialize<UserModel>(jsonResponse, options);

                    _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CurrentUser.Token);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserModel> GetProfileAsync()
        {
            // повертаємоюзера
            await Task.Delay(100);
            return CurrentUser ?? new UserModel();
        }
        public async Task<bool> UpdateProfileAsync(UserModel updatedUser)
        {
            try
            {
                string json = JsonSerializer.Serialize(updatedUser);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Відправляємо PUT-запит на сервер
                HttpResponseMessage response = await _httpClient.PutAsync("users/update", content);

                if (response.IsSuccessStatusCode)
                {
                    // Якщо сервер успішно оновив дані, синхронізуємо наш глобальний CurrentUser
                    if (CurrentUser != null)
                    {
                        CurrentUser.Login = updatedUser.Login;
                        CurrentUser.Phone = updatedUser.Phone;

                    }
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
            try
            {
                // Створюємо об'єкт нового юзера
                var newUser = new UserModel
                {
                    Login = username,
                    Email = email,
                    Password = password,
                    RegistrationDate = DateTime.Now.ToString("dd.MM.yyyy")
                };

                string json = JsonSerializer.Serialize(newUser);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("users/register", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}