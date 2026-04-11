using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClientAppe.ViewModels
{
    // Клас для способу оплати (краще тримати тут або в папці Models)
    public class PaymentMethod
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    public class CheckoutViewModel : ViewModelBase
    {
        // 1. ДЕТАЛІ ДОСТАВКИ
        public string StreetAddress { get; set; } = "м. Київ, вул. Хрещатик, 1";
        public string Entrance { get; set; }  // Під'їзд
        public string Floor { get; set; }     // Поверх
        public string Intercom { get; set; }  // Квартира/офіс
        public string CourierComment { get; set; }

        // 2. ОПЛАТА (ПОВЕРНУЛИ КОЛЕКЦІЮ)
        public ObservableCollection<PaymentMethod> PaymentMethods { get; set; }

        private PaymentMethod _selectedPaymentMethod;
        public PaymentMethod SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set { _selectedPaymentMethod = value; OnPropertyChanged(); }
        }

        // 3. ПІДСУМОК
        public string SelectedTip { get; set; } = "5%";
        public string FinalTotalToPay { get; set; } = "288.75 грн";

        // КОМАНДИ
        public ICommand SelectTipCommand { get; }
        public ICommand ConfirmOrderCommand { get; }

        public CheckoutViewModel()
        {
            // Ініціалізуємо методи оплати (тепер вони точно на місці!)
            PaymentMethods = new ObservableCollection<PaymentMethod>
            {
                new PaymentMethod { Name = "Банківська картка" },
                new PaymentMethod { Name = "Готівкою при отриманні" },
                new PaymentMethod { Name = "Apple Pay" },
                new PaymentMethod { Name = "Google Pay" }
            };

            // Ставимо перший метод за замовчуванням
            _selectedPaymentMethod = PaymentMethods[0];

            // Команди
            SelectTipCommand = new RelayCommand(tip =>
            {
                if (tip != null) SelectedTip = tip.ToString();
            });

            ConfirmOrderCommand = new RelayCommand(o =>
            {
                // Логіка підтвердження
            });
        }
    }
}