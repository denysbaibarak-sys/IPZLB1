using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using FoodOrderGuiClient.Models;

namespace FoodOrderGuiClient.Services
{
    public class NetworkClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl = "http://localhost:5000";

        public NetworkClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<RestaurantItem>> GetRestaurantsAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(new Uri($"{_serverUrl}/api/restaurants"));
                response.EnsureSuccessStatusCode();

                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var jsonSerializer = new DataContractJsonSerializer(typeof(List<RestaurantItem>));
                    return jsonSerializer.ReadObject(responseStream) as List<RestaurantItem> ?? new List<RestaurantItem>();
                }
            }
            catch (Exception)
            {
                return new List<RestaurantItem>();
            }
        }
    }
}