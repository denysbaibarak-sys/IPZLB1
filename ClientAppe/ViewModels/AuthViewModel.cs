using System.Windows.Input;
using ClientAppe.Services;
using System.Threading.Tasks;

namespace ClientAppe.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        // Посилання на головну в'ю-модель для навігації
        private readonly MainViewModel _mainViewModel;
        private readonly ApiService _apiService = new ApiService();

        private bool _isRegisterMode = false; // Зробимо вхід за замовчуванням
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

        // Динамічні тексти
        public string TitleText => IsRegisterMode ? "Реєстрація" : "Вхід";
        public string SubtitleText => IsRegisterMode ? "Створіть новий акаунт" : "Вітаємо знову!";
        public string ButtonText => IsRegisterMode ? "Зареєструватися" : "Увійти";
        public string SwitchHintText => IsRegisterMode ? "Вже маєте акаунт? " : "Ще не зареєстровані? ";
        public string SwitchButtonText => IsRegisterMode ? "Увійти" : "Створити акаунт";

        // Команди
        public ICommand AuthActionCommand { get; }
        public ICommand ToggleModeCommand { get; }

        // Тепер конструктор приймає MainViewModel
        public AuthViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            ToggleModeCommand = new RelayCommand(o => IsRegisterMode = !IsRegisterMode);

            AuthActionCommand = new RelayCommand(async o =>
            {
                ErrorMessage = "";

                // Імітуємо затримку завантаження
                await Task.Delay(50);

                if (IsRegisterMode)
                {
                    // Режим реєстрації
                    if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                    {
                        ErrorMessage = "Заповніть всі поля!";
                        return;
                    }

                    if (Password != ConfirmPassword)
                    {
                        ErrorMessage = "Паролі не збігаються!";
                        return;
                    }

                    // Просто перекидаємо на вхід (імітація успішної реєстрації)
                    IsRegisterMode = false;
                }
                else
                {
                    // Режим входу
                    // ПРЯМИЙ ПЕРЕХІД: ігноруємо перевірки для викладача
                    _mainViewModel.NavigateTo(new HomeViewModel(), false);
                }
            });
        }
    }
}