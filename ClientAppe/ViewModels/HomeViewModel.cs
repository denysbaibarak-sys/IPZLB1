using System;
using System.Windows.Input;
// ПРИБРАЛИ using System.Windows; — він тут зайвий і шкідливий

namespace ClientAppe.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string _deliveryAddress;
        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set { _deliveryAddress = value; OnPropertyChanged(); }
        }

        // Властивість для виводу повідомлень в інтерфейс (замість MessageBox)
        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand SelectCategoryCommand { get; }
        public ICommand OpenMapCommand { get; }

        public HomeViewModel()
        {
            DeliveryAddress = "м. Київ, вул. Хрещатик, 1";

            SelectCategoryCommand = new RelayCommand(category =>
            {
                // Замість MessageBox просто оновлюємо статус
                StatusMessage = $"Обрано категорію: {category}. Переходимо до списку ресторанів...";

                // В ЛБ 4 тут буде виклик: MainViewModel.Instance.CurrentView = new RestaurantsViewModel();
            });

            OpenMapCommand = new RelayCommand(o =>
            {
                StatusMessage = "Відкриття карти...";
            });
        }
    }
}