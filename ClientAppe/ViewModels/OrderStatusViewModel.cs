using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAppe.ViewModels
{
    public class OrderStatusViewModel : ViewModelBase
    {
        private string _currentStatus = "Очікування підтвердження...";
        public string CurrentStatus
        {
            get => _currentStatus;
            set { _currentStatus = value; OnPropertyChanged(); }
        }
    }
}
