using System.Collections.ObjectModel;
using System.Windows.Input;
using ClientAppe.Models;
using ClientAppe.Services;

namespace ClientAppe.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly ApiService _apiService = new ApiService();

        // Основна колекція, до якої прив'язаний ItemsControl у XAML
        private ObservableCollection<OrderModel> _orders;
        public ObservableCollection<OrderModel> Orders
        {
            get => _orders;
            set { _orders = value; OnPropertyChanged(); }
        }

        public ICommand SwitchTabCommand { get; }

        public OrdersViewModel()
        {
            SwitchTabCommand = new RelayCommand(tab => { /* Логіка перемикання */ });

            // Завантажуємо дані при створенні сторінки
            LoadOrders();
        }

        private async void LoadOrders()
        {
            var data = await _apiService.GetOrdersAsync();
            Orders = new ObservableCollection<OrderModel>(data);
        }
    }
}