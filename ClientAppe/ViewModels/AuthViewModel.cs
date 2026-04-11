using System.Windows.Input;
using ClientAppe.Services;

namespace ClientAppe.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        private readonly ApiService _apiService = new ApiService();

        private bool _isRegisterMode = true; // Початковий стан — Реєстрація
        public bool IsRegisterMode
        {
            get => _isRegisterMode;
            set
            {
                _isRegisterMode = value;
                OnPropertyChanged();
                // Оновлюємо всі тексти при зміні режиму
                OnPropertyChanged(nameof(TitleText));
                OnPropertyChanged(nameof(SubtitleText));
                OnPropertyChanged(nameof(ButtonText));
                OnPropertyChanged(nameof(SwitchHintText));
                OnPropertyChanged(nameof(SwitchButtonText));
            }
        }

        // Поля введення
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

        // Динамічні тексти для UI
        public string TitleText => IsRegisterMode ? "Реєстрація" : "Вхід";
        public string SubtitleText => IsRegisterMode ? "Створіть новий акаунт" : "Вітаємо знову!";
        public string ButtonText => IsRegisterMode ? "Зареєструватися" : "Увійти";
        public string SwitchHintText => IsRegisterMode ? "Вже маєте акаунт? " : "Ще не зареєстровані? ";
        public string SwitchButtonText => IsRegisterMode ? "Увійти" : "Створити акаунт";

        // Команди
        public ICommand AuthActionCommand { get; }
        public ICommand ToggleModeCommand { get; }

        public AuthViewModel()
        {
            // Перемикач режимів
            ToggleModeCommand = new RelayCommand(o => IsRegisterMode = !IsRegisterMode);

            // Основна дія (Вхід або Реєстрація)
            AuthActionCommand = new RelayCommand(async o =>
            {
                ErrorMessage = "";

                if (IsRegisterMode)
                {
                    // Логіка реєстрації
                    if (Password != ConfirmPassword) { ErrorMessage = "Паролі не збігаються!"; return; }
                    var success = await _apiService.RegisterAsync(Username, Email, Password);
                    if (success) IsRegisterMode = false; // Після реєстрації перекидаємо на вхід
                    else ErrorMessage = "Помилка реєстрації.";
                }
                else
                {
                    // Логіка входу
                    var user = await _apiService.LoginAsync(Email, Password);
                    if (user != null)
                    {
                        // Тут викликаємо команду навігації з MainViewModel (через біндинг або подію)
                    }
                    else ErrorMessage = "Невірний логін або пароль.";
                }
            });
        }
    }
}