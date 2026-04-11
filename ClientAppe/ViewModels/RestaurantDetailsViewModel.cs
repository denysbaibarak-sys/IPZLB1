using System.Collections.ObjectModel;
using System.Windows.Input;
using ClientAppe.Models;
using ClientAppe.Services;

namespace ClientAppe.ViewModels
{
    public class RestaurantDetailsViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        public RestaurantModel Restaurant { get; set; }

        public string RestaurantName => Restaurant?.Name ?? "Пузата Хата";
        public string Description { get; set; } = "Справжня українська домашня кухня...";

        private ObservableCollection<string> _categories;
        public ObservableCollection<string> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        private ObservableCollection<FoodModel> _menuItems;
        public ObservableCollection<FoodModel> MenuItems
        {
            get => _menuItems;
            set { _menuItems = value; OnPropertyChanged(); }
        }

        public ICommand SelectCategoryCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }

        // 1. ОСНОВНИЙ КОНСТРУКТОР (виправляє помилку в MainViewModel)
        public RestaurantDetailsViewModel(MainViewModel mainViewModel, RestaurantModel restaurant)
        {
            _mainViewModel = mainViewModel;
            Restaurant = restaurant;

            // Викликаємо ініціалізацію (твоя логіка)
            Initialize();
        }

        // 2. КОНСТРУКТОР БЕЗ ПАРАМЕТРІВ (щоб не ламався старий код, якщо десь лишився)
        public RestaurantDetailsViewModel()
        {
            Initialize();
        }

        // Твоя логіка наповнення даними
        private void Initialize()
        {
            Categories = new ObservableCollection<string> { "Перші страви", "Основні страви", "Напої" };

            // Беремо дані з Mock-сховища
            var menuData = MockDataStorage.GetMenu();
            MenuItems = new ObservableCollection<FoodModel>(menuData);

            SelectCategoryCommand = new RelayCommand(category => { /* фільтрація */ });

            AddToCartCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    // Логіка кошика
                }
            });
        }
    }
}