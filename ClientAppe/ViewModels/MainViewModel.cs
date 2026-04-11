using ClientAppe.Services;
using System.Windows.Input;

namespace ClientAppe.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CartService _cartService = new CartService();

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        // КОМАНДИ НАВІГАЦІЇ
        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToAuthCommand { get; }
        public ICommand NavigateToRestaurantsCommand { get; }
        public ICommand NavigateToRestaurantDetailsCommand { get; }
        public ICommand NavigateToCartCommand { get; }
        public ICommand NavigateToCheckoutCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public ICommand NavigateToOrdersCommand { get; }
        public ICommand NavigateToOrderStatusCommand { get; }

        public MainViewModel()
        {
            // 1. СТАРТУЄМО З ГОЛОВНОЇ (щоб відразу бачити дизайн)
            CurrentViewModel = new HomeViewModel();

            // 2. ІНІЦІАЛІЗАЦІЯ КОМАНД
            NavigateToHomeCommand = new RelayCommand(o => CurrentViewModel = new HomeViewModel());
            NavigateToAuthCommand = new RelayCommand(o => CurrentViewModel = new AuthViewModel());

            // Перехід до списку ресторанів (наприклад, з категорії "🍴 Ресторан")
            NavigateToRestaurantsCommand = new RelayCommand(o => CurrentViewModel = new RestaurantsViewModel());

            // Деталі ресторану
            NavigateToRestaurantDetailsCommand = new RelayCommand(o =>
            {
                // На ЛБ 4 тут буде передача вибраного об'єкта ресторану
                CurrentViewModel = new RestaurantDetailsViewModel();
            });

            // Кошик та оформлення
            NavigateToCartCommand = new RelayCommand(o => CurrentViewModel = new CartViewModel(_cartService));
            NavigateToCheckoutCommand = new RelayCommand(o => CurrentViewModel = new CheckoutViewModel());

            // Профіль та замовлення
            NavigateToProfileCommand = new RelayCommand(o => CurrentViewModel = new ProfileViewModel());
            NavigateToOrdersCommand = new RelayCommand(o => CurrentViewModel = new OrdersViewModel());

            // Статус замовлення (після натискання "Оформити")
            NavigateToOrderStatusCommand = new RelayCommand(o => CurrentViewModel = new OrderStatusViewModel());
        }
    }
}