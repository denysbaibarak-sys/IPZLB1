using System.Windows.Input;
using ClientAppe.Models;
using ClientAppe.Services;

namespace ClientAppe.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel; // Додаємо поле
        private UserModel _user;

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UserInitial));
            }
        }

        public string UserInitial => !string.IsNullOrEmpty(User?.Login) ? User.Login[0].ToString().ToUpper() : "?";

        public ICommand EditProfileCommand { get; }
        public ICommand SaveProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        // ОНОВЛЕНИЙ КОНСТРУКТОР: тепер приймає MainViewModel
        public ProfileViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            User = MockDataStorage.GetUserProfile();

            EditProfileCommand = new RelayCommand(o => { /* логіка */ });
            SaveProfileCommand = new RelayCommand(o => { /* логіка */ });

            LogoutCommand = new RelayCommand(o =>
            {
                // ВИПРАВЛЕНО: передаємо посилання на головну модель в AuthViewModel
                // Використовуємо NavigateTo з параметром false, щоб не можна було повернутися в профіль після виходу
                _mainViewModel.NavigateTo(new AuthViewModel(_mainViewModel), false);
            });
        }
    }
}