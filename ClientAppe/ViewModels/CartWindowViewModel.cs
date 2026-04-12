using ClientAppe.Services;
using System; // ОБОВ'ЯЗКОВО: для Action
using System.Collections.Generic;
using System.Windows.Input;

namespace ClientAppe.ViewModels
{
    public class CartWindowViewModel : ViewModelBase
    {
        private readonly CartService _cartService;
        private readonly Stack<ViewModelBase> _history = new Stack<ViewModelBase>();

        // Поле для зберігання нашого колбеку
        private readonly Action _onSuccessCallback;

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand GoBackCommand { get; }
        public ICommand NavigateToCheckoutCommand { get; }
        public ICommand ConfirmOrderCommand { get; }

        public Action RequestClose { get; set; }

        // ВИПРАВЛЕНИЙ КОНСТРУКТОР: тепер приймає 2 аргументи
        public CartWindowViewModel(CartService cartService, Action onSuccess)
        {
            _cartService = cartService;
            _onSuccessCallback = onSuccess; // Зберігаємо метод успіху

            // Початковий екран — кошик
            CurrentView = new CartViewModel(_cartService);

            // Навігація на оформлення
            NavigateToCheckoutCommand = new RelayCommand(o => {
                _history.Push(CurrentView);
                CurrentView = new CheckoutViewModel();
            });

            // Повернення назад або закриття вікна
            GoBackCommand = new RelayCommand(o => {
                if (_history.Count > 0) CurrentView = _history.Pop();
                else RequestClose?.Invoke();
            });

            // Команда для фінальної кнопки замовлення
            ConfirmOrderCommand = new RelayCommand(o => ConfirmOrder());
        }

        public void ConfirmOrder()
        {
            // 1. Закриваємо вікно кошика
            RequestClose?.Invoke();

            // 2. Викликаємо метод успіху в головному вікні
            _onSuccessCallback?.Invoke();
        }
    }
}