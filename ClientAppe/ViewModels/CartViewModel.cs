using ClientAppe.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ClientAppe.Services;
using System.Linq;

namespace ClientAppe.ViewModels
{
    public class CartViewModel : ViewModelBase
    {
        private readonly CartService _cartService;
        private readonly CartWindowViewModel _windowViewModel;

        public ObservableCollection<FoodModel> CartItems => _cartService.Items;

        public decimal ItemsCost => _cartService.GetTotal();
        public string DeliveryCost => "0";
        public decimal TotalCost => ItemsCost;
        public string ItemsCountText => $"Кошик ({CartItems.Count})";

        // ДОДАНО: властивість для перевірки, чи порожній кошик (щоб ховати/показувати кнопку оплати)
        public bool IsCartEmpty => CartItems.Count == 0;

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand ProceedToCheckoutCommand { get; }

        public CartViewModel(CartService cartService, CartWindowViewModel windowViewModel)
        {
            _cartService = cartService;
            _windowViewModel = windowViewModel;

            // ПІДПИСКА НА ПОДІЮ: коли кошик змінюється (навіть з меню), просто оновлюємо всі цифри!
            _cartService.CartUpdated += () =>
            {
                RefreshTotals();
            };

            // ЛОГІКА ВИДАЛЕННЯ
            RemoveItemCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    // Робимо кількість 0, щоб сервіс точно викинув страву
                    food.Quantity = 0;
                    _cartService.RemoveFromCart(food);
                }
            });

            // ЛОГІКА ПЛЮСА (Тепер дуже коротка!)
            IncreaseQuantityCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    // Сервіс сам зробить +1 і викличе CartUpdated
                    _cartService.AddToCart(food);
                }
            });

            // ЛОГІКА МІНУСА (Тепер дуже коротка!)
            DecreaseQuantityCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    // Сервіс сам зробить -1, а якщо стане 0 - сам видалить і викличе CartUpdated
                    _cartService.RemoveFromCart(food);
                }
            });

            // ПЕРЕХІД ДО ОПЛАТИ
            ProceedToCheckoutCommand = new RelayCommand(o =>
            {
                if (!IsCartEmpty)
                {
                    windowViewModel.NavigateToCheckout();
                }
            });
        }

        // Допоміжний метод для оновлення всіх цифр на екрані
        private void RefreshTotals()
        {
            OnPropertyChanged(nameof(ItemsCost));
            OnPropertyChanged(nameof(TotalCost));
            OnPropertyChanged(nameof(ItemsCountText));
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(IsCartEmpty)); // Виправляємо помилку IsCartEmpty
        }
    }
}