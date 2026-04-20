using ClientAppe.Models;
using ClientAppe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientAppe.ViewModels
{
    public class CartWindowViewModel : ViewModelBase
    {
        private readonly CartService _cartService;
        private readonly ApiService _apiService = new ApiService(); // Підключаємо мережу
        private readonly Stack<ViewModelBase> _history = new Stack<ViewModelBase>();
        private readonly Action _onSuccessCallback;

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand GoBackCommand { get; }
        public ICommand ConfirmOrderCommand { get; }

        public Action RequestClose { get; set; }

        public CartWindowViewModel(CartService cartService, Action onSuccess)
        {
            _cartService = cartService;
            _onSuccessCallback = onSuccess;

            // Передаємо посилання на ЦЕ вікно в CartViewModel, щоб він міг викликати NavigateToCheckout
            CurrentView = new CartViewModel(_cartService, this);

            GoBackCommand = new RelayCommand(o => {
                if (_history.Count > 0) CurrentView = _history.Pop();
                else RequestClose?.Invoke();
            });

            // Команда для фінальної кнопки (буде викликатися з CheckoutViewModel)
            ConfirmOrderCommand = new RelayCommand(async address => await ConfirmOrderAsync(address as string));
        }

        // Метод для переходу на сторінку оплати (викликається з CartViewModel)
        public void NavigateToCheckout()
        {
            _history.Push(CurrentView);
            // Передаємо сервіс і це вікно у Checkout, щоб він міг викликати ConfirmOrderCommand
            CurrentView = new CheckoutViewModel(_cartService, this);
        }

        // РЕАЛЬНА ВІДПРАВКА НА СЕРВЕР
        public async Task ConfirmOrderAsync(string deliveryAddress)
        {
            if (_cartService.Items.Count == 0) return;

            // 1. Формуємо об'єкт замовлення так, як цього чекає сервер
            var newOrder = new OrderModel
            {
                OrderedItems = _cartService.Items.ToList(),
                TotalPrice = (double)_cartService.GetTotal(), // Приводимо до double (або decimal, залежно від моделі)
                DeliveryAddress = string.IsNullOrWhiteSpace(deliveryAddress) ? "Самовивіз" : deliveryAddress,
                RestaurantName = "Доставка їжі", // Можна динамічно брати з обраних страв
                OrderDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            };

            // 2. Магія! Відправляємо JSON на сервер
            bool success = await _apiService.CreateOrderAsync(newOrder);

            if (success)
            {
                // 3. Сервер прийняв замовлення. Очищаємо кошик!
                _cartService.ClearCart(); // Якщо такого методу немає в CartService, доведеться додати `Items.Clear();`

                // 4. Закриваємо вікно кошика
                RequestClose?.Invoke();

                // 5. Кажемо головному вікну: "Усе супер, покажи статус замовлення!"
                _onSuccessCallback?.Invoke();
            }
            else
            {
                // Тут можна додати вивід помилки в UI, але для початку достатньо логу
                System.Diagnostics.Debug.WriteLine("Помилка сервера: Замовлення не створено.");
            }
        }
    }
}