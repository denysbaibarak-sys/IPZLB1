using System.Collections.ObjectModel;
using System.Linq;
using ClientAppe.Models;

namespace ClientAppe.Services
{
    public class CartService
    {
        // Список страв у кошику (ObservableCollection, щоб UI бачив зміни автоматично)
        public ObservableCollection<FoodModel> Items { get; } = new ObservableCollection<FoodModel>();

        // Додати страву
        public void AddToCart(FoodModel food)
        {
            Items.Add(food);
        }

        // Видалити страву
        public void RemoveFromCart(FoodModel food)
        {
            Items.Remove(food);
        }

        // Очистити кошик
        public void ClearCart()
        {
            Items.Clear();
        }

        // Порахувати загальну суму
        public decimal GetTotal()
        {
            return Items.Sum(x => x.Price);
        }
    }
}