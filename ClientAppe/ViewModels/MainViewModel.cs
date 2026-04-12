using ClientAppe.Models;
using ClientAppe.Services;
using ClientAppe.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ClientAppe.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // 1. СЕРВІСИ ТА СТЕК НАВІГАЦІЇ
        private readonly CartService _cartService = new CartService();
        private readonly Stack<ViewModelBase> _history = new Stack<ViewModelBase>();
        private CartWindow _openedCartWindow;

        // 2. ВЛАСТИВОСТІ СТАНУ
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        private bool _isSuccessMessageVisible;
        public bool IsSuccessMessageVisible
        {
            get => _isSuccessMessageVisible;
            set { _isSuccessMessageVisible = value; OnPropertyChanged(); }
        }

        // 3. КОМАНДИ
        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToAuthCommand { get; }
        public ICommand NavigateToRestaurantsCommand { get; }
        public ICommand NavigateToCartCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public ICommand NavigateToOrdersCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand CloseSuccessMessageCommand { get; }

        public MainViewModel()
        {
            // Ініціалізація команд
            NavigateToHomeCommand = new RelayCommand(o => NavigateTo(new HomeViewModel()));
            NavigateToAuthCommand = new RelayCommand(o => NavigateTo(new AuthViewModel(this)));

            // Для ресторанів передаємо посилання на MainViewModel (this), щоб працював перехід в деталі
            NavigateToRestaurantsCommand = new RelayCommand(o => NavigateTo(new RestaurantsViewModel(this)));

            // Кошик тепер відкривається в окремому вікні
            NavigateToCartCommand = new RelayCommand(o => OpenCartWindow());

            NavigateToProfileCommand = new RelayCommand(o => NavigateTo(new ProfileViewModel(this)));
            NavigateToOrdersCommand = new RelayCommand(o => NavigateTo(new OrdersViewModel()));

            GoBackCommand = new RelayCommand(o => GoBack());
            CloseSuccessMessageCommand = new RelayCommand(o => IsSuccessMessageVisible = false);

            // Стартуємо з головної сторінки
            NavigateTo(new AuthViewModel(this), false);
        }

        // 4. МЕТОДИ НАВІГАЦІЇ (ГОЛОВНЕ ВІКНО)
        public void NavigateTo(ViewModelBase nextViewModel, bool saveToHistory = true)
        {
            if (nextViewModel == null) return;

            // Зберігаємо поточну сторінку в стек перед переходом
            if (saveToHistory && CurrentViewModel != null)
            {
                _history.Push(CurrentViewModel);
            }

            CurrentViewModel = nextViewModel;
        }

        public void GoBack()
        {
            if (_history.Count > 0)
            {
                // Дістаємо попередню сторінку зі стеку
                CurrentViewModel = _history.Pop();
            }
        }

        // Спеціальний перехід до деталей ресторану
        public void NavigateToDetails(RestaurantModel restaurant)
        {
            if (restaurant != null)
            {
                NavigateTo(new RestaurantDetailsViewModel(this, restaurant));
            }
        }

        // 5. ЛОГІКА МОДУЛЬНОГО ВІКНА КОШИКА
        public void OpenCartWindow()
        {
            // Перевірка, щоб не відкрити кошик двічі
            if (_openedCartWindow != null)
            {
                _openedCartWindow.Activate();
                return;
            }

            // Створюємо VM для вікна кошика, передаємо сервіс та "callback" успіху
            var cartWindowVM = new CartWindowViewModel(_cartService, OnOrderSuccess);

            _openedCartWindow = new CartWindow
            {
                DataContext = cartWindowVM
            };

            // Прив'язуємо закриття вікна до запиту з VM
            cartWindowVM.RequestClose = () => _openedCartWindow?.Close();

            _openedCartWindow.Closed += (s, e) => _openedCartWindow = null;
            _openedCartWindow.Show();
        }

        // Метод, який викликається автоматично при успішному замовленні у вікні кошика
        private void OnOrderSuccess()
        {
            // Показуємо наш кастомний Overlay (MainWindow.axaml)
            IsSuccessMessageVisible = true;

            // Очищуємо кошик через сервіс
            _cartService.ClearCart();

            // Повертаємо головне вікно на головну сторінку
            NavigateTo(new HomeViewModel(), false);

            // Очищуємо історію навігації головного вікна, щоб "Назад" не вела до оформлення
            _history.Clear();
        }
    }
}