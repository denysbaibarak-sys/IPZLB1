using System.Collections.Generic;
using ClientAppe.Models;

namespace ClientAppe.Services
{
    public static class MockDataStorage
    {
        // Список ресторанів для головної сторінки та каталогу
        public static List<RestaurantModel> GetRestaurants() => new()
        {
            new RestaurantModel {
                Name = "Пузата Хата",
                Rating = "4.8",
                DeliveryTime = "30-45 хв",
                Distance = "1.2 км",
                Description = "Справжня українська домашня кухня. Гарячі борщі та вареники.",
                Address = "вул. Басейна, 1/2"
            },
            new RestaurantModel {
                Name = "Pizza Mafia",
                Rating = "4.6",
                DeliveryTime = "40-50 хв",
                Distance = "2.5 км",
                Description = "Найкраща піца у місті на дровах.",
                Address = "пр-т Перемоги, 45"
            },
            new RestaurantModel {
                Name = "Суші Майстер",
                Rating = "4.9",
                DeliveryTime = "45-60 хв",
                Distance = "3.1 км",
                Description = "Свіжі роли та сети для великої компанії.",
                Address = "вул. Хрещатик, 20"
            }
        };

        // Детальне меню для обраного ресторану
        public static List<FoodModel> GetMenu() => new()
        {
            new FoodModel { Name = "Борщ український", Price = 85, Description = "Насичений червоний борщ зі свининою та сметаною." },
            new FoodModel { Name = "Солянка м'ясна", Price = 95, Description = "Густий суп з копченостями, оливками та лимоном." },
            new FoodModel { Name = "Піца Маргарита", Price = 190, Description = "Класична піца з томатами та моцарелою." },
        };

        // Дані профілю
        public static UserModel GetUserProfile() => new()
        {
            Login = "Debik",
            Email = "example@gmail.com",
            Phone = "+380111111111",
            RegistrationDate = "17 березня 2026 р.",
            Address = "м. Київ, вул. Хрещатик, 1"
        };
        public static List<OrderModel> GetOrders() => new()
{
    new OrderModel {
        RestaurantName = "Пузата Хата",
        ItemsSummary = "Борщ український х1, Солянка х1",
        TotalPrice = 193.2,
        OrderDate = "19 березня 2026 р.",
        Status = "В обробці"
    },
    new OrderModel {
        RestaurantName = "Pizza Mafia",
        ItemsSummary = "Піца Маргарита х2",
        TotalPrice = 380,
        OrderDate = "18 березня 2026 р.",
        Status = "Доставлено"
    }
};
    }
}