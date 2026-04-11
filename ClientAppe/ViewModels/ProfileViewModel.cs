using System.Windows.Input;
using ClientAppe.Models;
using ClientAppe.Services;

namespace ClientAppe.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        // Використовуємо об'єкт моделі, щоб відповідати біндингам у XAML
        private UserModel _user;
        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
                // Повідомляємо, що ініціал теж змінився
                OnPropertyChanged(nameof(UserInitial));
            }
        }

        // Логіка для аватарки (перша літера логіна)
        public string UserInitial => !string.IsNullOrEmpty(User?.Login) ? User.Login[0].ToString().ToUpper() : "?";

        // Команди
        public ICommand EditProfileCommand { get; }
        public ICommand SaveProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel()
        {
            // ЗАВАНТАЖЕННЯ ДАНИХ:
            // Тепер ми не просто ставимо "Guest", а беремо дані з нашого Mock-сховища
            User = MockDataStorage.GetUserProfile();

            EditProfileCommand = new RelayCommand(o =>
            {
                // Логіка редагування
            });

            SaveProfileCommand = new RelayCommand(o =>
            {
                // Логіка збереження
            });

            LogoutCommand = new RelayCommand(o =>
            {
                // Логіка виходу (наприклад, перехід на AuthView)
                if (o is MainViewModel mainVM)
                {
                    mainVM.CurrentViewModel = new AuthViewModel();
                }
            });
        }
    }
}