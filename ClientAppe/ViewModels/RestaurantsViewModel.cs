using ClientAppe.Models;
using ClientAppe.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClientAppe.ViewModels
{
    public class RestaurantsViewModel : ViewModelBase
    {
        private readonly ApiService _apiService = new ApiService();

        private ObservableCollection<RestaurantModel> _restaurants;
        public ObservableCollection<RestaurantModel> Restaurants
        {
            get => _restaurants;
            set { _restaurants = value; OnPropertyChanged(); }
        }

        private string _foundCountText;
        public string FoundCountText
        {
            get => _foundCountText;
            set { _foundCountText = value; OnPropertyChanged(); }
        }

        public ICommand FilterCategoryCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand SelectRestaurantCommand { get; }

        public RestaurantsViewModel()
        {
            // 1. Спочатку ініціалізуємо команди
            FilterCategoryCommand = new RelayCommand(category => { /* Логіка фільтрації */ });
            SortCommand = new RelayCommand(sortType => { /* Логіка сортування */ });

            SelectRestaurantCommand = new RelayCommand(restaurant =>
            {
                if (restaurant is RestaurantModel selected)
                {
                    // В ЛБ 4 тут буде перехід: 
                    // MainViewModel.CurrentViewModel = new RestaurantDetailsViewModel(selected);
                }
            });

            // 2. А потім запускаємо завантаження даних
            LoadData();
        }

        private async void LoadData()
        {
            // Викликаємо метод з ApiService, який імітує мережу
            var data = await _apiService.GetRestaurantsAsync();

            // Оновлюємо список
            Restaurants = new ObservableCollection<RestaurantModel>(data);

            // Оновлюємо лічильник
            FoundCountText = $"Знайдено {Restaurants.Count} закладів";
        }
    }
}