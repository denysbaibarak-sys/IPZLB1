using System.Collections.Generic;
using System.Linq;

namespace ClientAppe.Models
{
    public class CartModel
    {
        // Список страв у кошику
        public List<FoodModel> Items { get; set; } = new List<FoodModel>();

        // Сума тепер також у decimal, щоб збігатися з типом ціни страви
        public decimal TotalPrice => Items.Sum(item => item.Price);
    }
}