using System.Collections.Generic;
using System.Linq;

namespace ClientAppe.Models
{
    public class CartModel
    {
        // Список страв у кошику
        public List<FoodModel> Items { get; set; } = new List<FoodModel>();

        // Автоматичний підрахунок суми
        public double TotalPrice => Items.Sum(item => item.Price);
    }
}