using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using FoodOrderGuiClient.Models;
using FoodOrderGuiClient.Services;

namespace FoodOrderGuiClient
{
    public partial class MainWindow : Window
    {
        private readonly NetworkClient _client;

        public MainWindow()
        {
            InitializeComponent();
            _client = new NetworkClient();
        }

        private async void LoadButton_Click(object? sender, RoutedEventArgs e)
        {
            if (StatusText != null) StatusText.Text = "Статус: Завантаження даних з сервера...";
            List<RestaurantItem> restaurants = await _client.GetRestaurantsAsync();
            
            if (RestaurantsList != null) RestaurantsList.ItemsSource = restaurants;
            if (StatusText != null)
            {
                StatusText.Text = restaurants.Count > 0 
                    ? $"Статус: Успішно завантажено {restaurants.Count} ресторанів." 
                    : "Статус: Помилка завантаження або порожній список.";
            }
        }

        private void RestaurantsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (RestaurantsList?.SelectedItem is RestaurantItem selectedRestaurant)
            {
                if (MenuList != null) MenuList.ItemsSource = selectedRestaurant.Menu;
                if (StatusText != null) StatusText.Text = $"Статус: Обрано ресторан {selectedRestaurant.Name}";
            }
        }

        private void MenuList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (MenuList?.SelectedItem is string selectedDish && 
                RestaurantsList?.SelectedItem is RestaurantItem selectedRestaurant)
            {
                if (StatusText != null) StatusText.Text = $"Статус: Замовлення сформовано! Страва: {selectedDish} ({selectedRestaurant.Name})";
                MenuList.SelectedItem = null;
            }
        }
    }
}