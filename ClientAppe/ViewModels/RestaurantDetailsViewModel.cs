using System.Collections.ObjectModel;
using System.Windows.Input;
using ClientAppe.Models;
using ClientAppe.Services; // Додай для доступу до MockDataStorage

namespace ClientAppe.ViewModels
{
    public class RestaurantDetailsViewModel : ViewModelBase
    {
        public string RestaurantName { get; set; } = "Пузата Хата";
        public string Description { get; set; } = "Справжня українська домашня кухня...";

        // Поле для категорій (виправляє помилку MC3073)
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

        public ICommand SelectCategoryCommand { get; }
        public ICommand AddToCartCommand { get; }

        public RestaurantDetailsViewModel()
        {
            // Ініціалізуємо категорії
            Categories = new ObservableCollection<string> { "Перші страви", "Основні страви", "Напої" };

            // Беремо дані з нашого Mock-сховища
            var menuData = MockDataStorage.GetMenu();
            MenuItems = new ObservableCollection<FoodModel>(menuData);

            SelectCategoryCommand = new RelayCommand(category => { /* фільтрація */ });

            AddToCartCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    // Тут буде логіка додавання в CartService
                }
            });
        }
    }
}