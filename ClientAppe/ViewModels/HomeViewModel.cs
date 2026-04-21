using System;
using System.Windows.Input;

namespace ClientAppe.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        // Посилання на головну модель для навігації
        private readonly MainViewModel _mainViewModel;

        private string _deliveryAddress;
        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set { _deliveryAddress = value; OnPropertyChanged(); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand SelectCategoryCommand { get; }
        public ICommand OpenMapCommand { get; }

        // Конструктор приймає MainViewModel
        public HomeViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            DeliveryAddress = "м. Київ, вул. Хрещатик, 1";

            SelectCategoryCommand = new RelayCommand(category =>
            {
                StatusMessage = $"Переходимо до закладів: {category}...";

                // Відкриваємо сторінку ресторанів!
                // Передаємо _mainViewModel далі, щоб з ресторанів можна було йти ще далі
                _mainViewModel.NavigateTo(new RestaurantsViewModel(_mainViewModel), true);
            });

            OpenMapCommand = new RelayCommand(o =>
            {
                StatusMessage = "Відкриття карти...";
            });
        }
    }
}