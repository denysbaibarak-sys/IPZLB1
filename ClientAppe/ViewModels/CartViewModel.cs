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

        // Прив'язуємо колекцію прямо до сервісу
        public ObservableCollection<FoodModel> CartItems => _cartService.Items;

        // Динамічні властивості для підрахунку суми
        public decimal ItemsCost => _cartService.GetTotal();
        public string DeliveryCost => "Безкоштовно";
        public decimal TotalCost => ItemsCost;

        // Для заголовка "Кошик (2)"
        public string ItemsCountText => $"Кошик ({CartItems.Count})";

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand ProceedToCheckoutCommand { get; }

        public CartViewModel(CartService cartService)
        {
            _cartService = cartService;

            // ОБ'ЄДНАНО: Підписуємося на всі зміни в кошику в одному місці
            _cartService.Items.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(ItemsCost));
                OnPropertyChanged(nameof(TotalCost));
                OnPropertyChanged(nameof(ItemsCountText)); // Тепер лічильник оновиться автоматично
                OnPropertyChanged(nameof(CartItems));    // На всякий випадок для UI
            };

            // Команда видалення
            RemoveItemCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    _cartService.RemoveFromCart(food);
                }
            });

            ProceedToCheckoutCommand = new RelayCommand(o =>
            {
                // Логіка переходу на оплату
            });

            // Заглушки для ЛБ 4
            IncreaseQuantityCommand = new RelayCommand(o => { });
            DecreaseQuantityCommand = new RelayCommand(o => { });
        }
    }
}