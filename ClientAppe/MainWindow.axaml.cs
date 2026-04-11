using Avalonia.Controls;
using ClientAppe.ViewModels;

namespace ClientAppe.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}