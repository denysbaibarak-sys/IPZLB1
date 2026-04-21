using System;
using System.Linq;
using System.Threading.Tasks;
using ClientAppe.Services;

namespace ClientAppe.ViewModels
{
    public class OrderStatusViewModel : ViewModelBase
    {
        private readonly ApiService _apiService = new ApiService();
        private bool _isPolling = true;

        private string _currentStatus = "Очікування підтвердження...";
        public string CurrentStatus
        {
            get => _currentStatus;
            set { _currentStatus = value; OnPropertyChanged(); }
        }

        public OrderStatusViewModel()
        {
            // Запускаємо процес постійного опитування сервера (Polling)
            StartPollingAsync();
        }

        private async void StartPollingAsync()
        {
            while (_isPolling)
            {
                // Чекаємо 5 секунд перед кожним запитом (щоб не "покласти" сервер)
                await Task.Delay(5000);

                try
                {
                    var allOrders = await _apiService.GetOrdersAsync();
                    var myLastOrder = allOrders.LastOrDefault();

                    if (myLastOrder != null && myLastOrder.Status != CurrentStatus)
                    {
                        // Якщо сервер повернув новий статус — оновлюємо UI
                        CurrentStatus = myLastOrder.Status;

                        // Якщо замовлення виконано, можна зупинити Polling
                        if (CurrentStatus == "Доставлено")
                        {
                            _isPolling = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Polling error: " + ex.Message);
                }
            }
        }

        // Метод, щоб зупинити таймер, якщо користувач пішов з цієї сторінки
        public void StopPolling()
        {
            _isPolling = false;
        }
    }
}