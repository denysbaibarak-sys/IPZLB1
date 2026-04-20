using System.Windows.Input;
using ClientAppe.Models;
using ClientAppe.Services;

namespace ClientAppe.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ApiService _apiService = new ApiService();

        private UserModel _user;
        public UserModel User
        {
            get => _user;
            set { _user = value; OnPropertyChanged(); OnPropertyChanged(nameof(UserInitial)); }
        }

        // --- ДОДАНО ДЛЯ РЕДАГУВАННЯ ---
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        private string _editLogin;
        public string EditLogin
        {
            get => _editLogin;
            set { _editLogin = value; OnPropertyChanged(); }
        }

        private string _editPhone;
        public string EditPhone
        {
            get => _editPhone;
            set { _editPhone = value; OnPropertyChanged(); }
        }

        private string _editPassword;
        public string EditPassword
        {
            get => _editPassword;
            set { _editPassword = value; OnPropertyChanged(); }
        }
        // ------------------------------

        public string UserInitial => !string.IsNullOrEmpty(User?.Login) ? User.Login[0].ToString().ToUpper() : "?";

        public ICommand EditProfileCommand { get; }
        public ICommand CancelEditCommand { get; } // Додано
        public ICommand SaveProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoadProfile();

            // 1. Відкриваємо вікно редагування
            EditProfileCommand = new RelayCommand(o => {
                EditLogin = User.Login;
                EditPhone = User.Phone; // Переконайся, що в UserModel є поле Phone
                EditPassword = "";
                IsEditing = true;
            });

            // 2. Скасовуємо
            CancelEditCommand = new RelayCommand(o => { IsEditing = false; });

            // 3. Зберігаємо
            SaveProfileCommand = new RelayCommand(async o => {
                var updatedUser = new UserModel
                {
                    Id = User.Id,
                    Login = EditLogin,
                    Phone = EditPhone,
                    Password = EditPassword
                };

                bool success = await _apiService.UpdateProfileAsync(updatedUser);
                if (success)
                {
                    User.Login = EditLogin;
                    User.Phone = EditPhone;
                    OnPropertyChanged(nameof(User));
                    IsEditing = false;
                }
            });

            LogoutCommand = new RelayCommand(o =>
            {
                ApiService.CurrentUser = null;
                _mainViewModel.NavigateTo(new AuthViewModel(_mainViewModel), false);
            });
        }

        private async void LoadProfile()
        {
            User = await _apiService.GetProfileAsync() ?? new UserModel { Login = "Гість" };
        }
    }
}