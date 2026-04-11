using System.Collections.Generic;

namespace ClientAppe.Models
{
    public class RestaurantModel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        // Додаємо ці поля для нашого нового дизайну
        public string Rating { get; set; }
        public string DeliveryTime { get; set; }
        public string Distance { get; set; }
        public string Description { get; set; }

        public List<FoodModel> Menu { get; set; }
    }
}