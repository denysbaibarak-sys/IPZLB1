using System;
using System.Collections.Generic;

namespace ClientAppe.Models
{
    public class OrderModel
    {
        public string OrderId { get; set; }
        public string RestaurantName { get; set; }
        public string OrderDate { get; set; } // Змінюємо з DateTime на string для спрощення
        public List<FoodModel> OrderedItems { get; set; }
        public string ItemsSummary { get; set; }
        public double TotalPrice { get; set; } // Перейменували з TotalAmount на TotalPrice
        public string Status { get; set; }
        public string DeliveryAddress { get; set; }
    }
}