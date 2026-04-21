using System.Windows.Input;
using ClientAppe.Services;
using System.Threading.Tasks;

namespace ClientAppe.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ApiService _apiService = new ApiService();

        private bool _isRegisterMode = false;
        public bool IsRegisterMode
        {
            get => _isRegisterMode;
            set
            {
                _isRegisterMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TitleText));
                OnPropertyChanged(nameof(SubtitleText));
                OnPropertyChanged(nameof(ButtonText));
                OnPropertyChanged(nameof(SwitchHintText));
                OnPropertyChanged(nameof(SwitchButtonText));
            }
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public string TitleText => IsRegisterMode ? "Реєстрація" : "Вхід";
        public string SubtitleText => IsRegisterMode ? "Створіть новий акаунт" : "Вітаємо знову!";
        public string ButtonText => IsRegisterMode ? "Зареєструватися" : "Увійти";
        public string SwitchHintText => IsRegisterMode ? "Вже маєте акаунт? " : "Ще не зареєстровані? ";
        public string SwitchButtonText => IsRegisterMode ? "Увійти" : "Створити акаунт";

        public ICommand AuthActionCommand { get; }
        public ICommand ToggleModeCommand { get; }

        public AuthViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            ToggleModeCommand = new RelayCommand(o => {
                IsRegisterMode = !IsRegisterMode;
                ErrorMessage = "";
            });

            // Оновлена команда з реальною мережевою логікою
            AuthActionCommand = new RelayCommand(async o =>
            {
                ErrorMessage = "";

                if (IsRegisterMode)
                {
                    // РЕЄСТРАЦІЯ
                    if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Username))
                    {
                        ErrorMessage = "Заповніть всі поля!";
                        return;
                    }

                    if (Password != ConfirmPassword)
                    {
                        ErrorMessage = "Паролі не збігаються!";
                        return;
                    }

                    // Викликаємо сервер для збереження юзера
                    bool success = await _apiService.RegisterAsync(Username, Email, Password);

                    if (success)
                    {
                        IsRegisterMode = false;
                        ErrorMessage = "Реєстрація успішна! Тепер увійдіть.";
                    }
                    else
                    {
                        ErrorMessage = "Помилка! Можливо, користувач вже існує.";
                    }
                }
                else
                {
                    // ВХІД
                    if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    {
                        ErrorMessage = "Введіть логін та пароль!";
                        return;
                    }

                    // Викликаємо сервер для перевірки даних
                    bool success = await _apiService.LoginAsync(Username, Password);

                    if (success)
                    {
                        // Якщо сервер підтвердив пароль і повернув дані юзера, пускаємо в додаток
                        _mainViewModel.NavigateTo(new HomeViewModel(_mainViewModel), false);
                    }
                    else
                    {
                        ErrorMessage = "Невірний логін або пароль!";
                    }
                }
            });
        }
    }
}