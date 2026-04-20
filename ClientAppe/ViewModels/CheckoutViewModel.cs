using ClientAppe.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ClientAppe.Services;
using System.Linq;
using System;

namespace ClientAppe.ViewModels
{
    public class PaymentMethod
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    public class CheckoutViewModel : ViewModelBase
    {
        private readonly CartService _cartService;
        private readonly CartWindowViewModel _windowViewModel;

        // 1. ДЕТАЛІ ДОСТАВКИ (Прив'язані до UI)
        public string StreetAddress { get; set; } = "";
        public string Entrance { get; set; }
        public string Floor { get; set; }
        public string Intercom { get; set; }
        public string CourierComment { get; set; }

        // 2. ОПЛАТА
        public ObservableCollection<PaymentMethod> PaymentMethods { get; set; }

        private PaymentMethod _selectedPaymentMethod;
        public PaymentMethod SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set { _selectedPaymentMethod = value; OnPropertyChanged(); }
        }

        // 3. ПІДСУМОК ТА ЧАЙОВІ
        private string _selectedTip = "0%";
        public string SelectedTip
        {
            get => _selectedTip;
            set
            {
                _selectedTip = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FinalTotalToPay)); // Перераховуємо суму при зміні чайових
            }
        }

        // РЕАЛЬНИЙ ПІДРАХУНОК СУМИ
        public string FinalTotalToPay
        {
            get
            {
                decimal subtotal = _cartService.GetTotal();
                decimal tipPercent = 0;

                // Парсимо чайові (наприклад, з "5%" робимо 0.05)
                if (decimal.TryParse(SelectedTip.Replace("%", ""), out decimal val))
                {
                    tipPercent = val / 100;
                }

                decimal total = subtotal + (subtotal * tipPercent);
                return $"{total:N2} грн";
            }
        }

        public ICommand SelectTipCommand { get; }
        public ICommand ConfirmOrderCommand { get; }

        // ОНОВЛЕНИЙ КОНСТРУКТОР: приймає сервіс та вікно
        public CheckoutViewModel(CartService cartService, CartWindowViewModel windowViewModel)
        {
            _cartService = cartService;
            _windowViewModel = windowViewModel;

            // Ініціалізація методів оплати
            PaymentMethods = new ObservableCollection<PaymentMethod>
            {
                new PaymentMethod { Name = "Банківська картка" },
                new PaymentMethod { Name = "Готівкою при отриманні" },
                new PaymentMethod { Name = "Apple Pay" },
                new PaymentMethod { Name = "Google Pay" }
            };

            _selectedPaymentMethod = PaymentMethods[0];

            // Команда вибору чайових
            SelectTipCommand = new RelayCommand(tip =>
            {
                if (tip != null) SelectedTip = tip.ToString();
            });

            // ПІДТВЕРДЖЕННЯ ЗАМОВЛЕННЯ
            ConfirmOrderCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrWhiteSpace(StreetAddress))
                {
                    // Можна додати ErrorMessage, якщо вулиця порожня
                    return;
                }

                // Збираємо повну адресу в один рядок для сервера
                string fullAddress = $"{StreetAddress}, під'їзд {Entrance}, пов. {Floor}, кв/офіс {Intercom}";
                if (!string.IsNullOrEmpty(CourierComment))
                    fullAddress += $" (Коментар: {CourierComment})";

                // Викликаємо метод відправки, який ми написали в CartWindowViewModel
                _windowViewModel.ConfirmOrderCommand.Execute(fullAddress);
            });
        }
    }
}