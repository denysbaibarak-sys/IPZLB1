using System.Collections.ObjectModel;
using System.Windows.Input;
using ClientAppe.Models;
using ClientAppe.Services;
using System.Linq;
using System.Collections.Generic;

namespace ClientAppe.ViewModels
{
    public class RestaurantDetailsViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly CartService _cartService;

        // Зберігаємо повне меню, щоб не втратити страви при фільтрації
        private List<FoodModel> _allMenuItems = new List<FoodModel>();

        public RestaurantModel Restaurant { get; set; }

        public string RestaurantName => Restaurant?.Name ?? "Невідомий заклад";
        public string Description => Restaurant?.Description ?? "Опис відсутній";

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
        public ICommand GoBackCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public RestaurantDetailsViewModel(MainViewModel mainViewModel, RestaurantModel restaurant, CartService cartService)
        {
            _mainViewModel = mainViewModel;
            Restaurant = restaurant;
            _cartService = cartService;

            GoBackCommand = new RelayCommand(o => _mainViewModel.GoBack());

            // ФІЛЬТРАЦІЯ
            SelectCategoryCommand = new RelayCommand(category =>
            {
                if (category is string catStr)
                {
                    if (catStr == "Всі")
                    {
                        MenuItems = new ObservableCollection<FoodModel>(_allMenuItems);
                    }
                    else
                    {
                        var filtered = _allMenuItems.Where(f =>
                            f.Category != null &&
                            f.Category.Trim().ToLower() == catStr.Trim().ToLower()
                        ).ToList();

                        MenuItems = new ObservableCollection<FoodModel>(filtered);
                    }
                }
            });
            RemoveFromCartCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    _cartService.RemoveFromCart(food);

                    // шукаємо цю страву в кошику і оновлюємо цифру
                    var itemInCart = _cartService.Items.FirstOrDefault(c => c.Name == food.Name);
                    food.Quantity = itemInCart != null ? itemInCart.Quantity : 0;
                }
            });

            AddToCartCommand = new RelayCommand(item =>
            {
                if (item is FoodModel food)
                {
                    _cartService.CurrentRestaurantId = this.Restaurant.Id;
                    _cartService.AddToCart(food);

                    // після додавання оновлюємо цифру
                    var itemInCart = _cartService.Items.FirstOrDefault(c => c.Name == food.Name);
                    food.Quantity = itemInCart != null ? itemInCart.Quantity : 0;
                }
            });
            LoadData();
        }

        private void LoadData()
        {
            // Категорії меню
            Categories = new ObservableCollection<string> { "Всі", "Перші страви", "Основні страви", "Напої" };

            if (Restaurant != null && Restaurant.Menu != null)
            {
                _allMenuItems = Restaurant.Menu.ToList();

                foreach (var food in _allMenuItems)
                {
                    var itemInCart = _cartService.Items.FirstOrDefault(c => c.Name == food.Name);
                    food.Quantity = itemInCart != null ? itemInCart.Quantity : 0;
                }

                MenuItems = new ObservableCollection<FoodModel>(_allMenuItems);
            }
            else
            {
                _allMenuItems = new List<FoodModel>();
                MenuItems = new ObservableCollection<FoodModel>();
            }
        }
    }
}