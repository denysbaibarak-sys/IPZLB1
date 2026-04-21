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


        public bool IsCartEmpty => CartItems.Count == 0;

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand ProceedToCheckoutCommand { get; }

        public CartViewModel(CartService cartService, CartWindowViewModel windowViewModel)
        {
            _cartService = cartService;
            _windowViewModel = windowViewModel;

            _cartService.CartUpdated += () =>
            {
                RefreshTotals();
            };

            // ЛОГІКА ВИДАЛЕННЯ
            RemoveItemCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    food.Quantity = 0;
                    _cartService.RemoveFromCart(food);
                }
            });

            IncreaseQuantityCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    _cartService.AddToCart(food);
                }
            });

            DecreaseQuantityCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
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
            OnPropertyChanged(nameof(IsCartEmpty));
        }
    }
}