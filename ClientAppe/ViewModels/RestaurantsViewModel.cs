using ClientAppe.Models;
using ClientAppe.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;

namespace ClientAppe.ViewModels
{
    public class RestaurantsViewModel : ViewModelBase
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly MainViewModel _mainViewModel;

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

        // Команди обов'язково мають бути public
        public ICommand FilterCategoryCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand NavigateToDetailsCommand { get; }

        public RestaurantsViewModel(MainViewModel mainVM)
        {
            _mainViewModel = mainVM ?? throw new ArgumentNullException(nameof(mainVM));

            // Ініціалізуємо команди відразу
            FilterCategoryCommand = new RelayCommand(category =>
            {
                // Тут буде фільтрація в ЛБ 4
                Console.WriteLine($"Фільтр: {category}");
            });

            SortCommand = new RelayCommand(sortType =>
            {
                // Тут буде сортування в ЛБ 4
                Console.WriteLine($"Сортування: {sortType}");
            });

            // Ця команда відповідає за клік по картці
            NavigateToDetailsCommand = new RelayCommand(param =>
            {
                // Перевіряємо, що прийшов саме об'єкт ресторану
                if (param is RestaurantModel selected)
                {
                    _mainViewModel.NavigateToDetails(selected);
                }
            });

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var data = await _apiService.GetRestaurantsAsync();
                if (data != null)
                {
                    Restaurants = new ObservableCollection<RestaurantModel>(data);
                    FoundCountText = $"Знайдено {Restaurants.Count} закладів";
                }
            }
            catch (Exception ex)
            {
                FoundCountText = "Помилка зв'язку з сервером";
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}