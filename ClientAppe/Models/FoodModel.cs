using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientAppe.Models
{
    // Обов'язково додаємо INotifyPropertyChanged
    public class FoodModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value) // перевірка, щоб не робити зайвої роботи
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsInCart));
                    OnPropertyChanged(nameof(IsNotInCart));

                    // 🌟 ДОДАЄМО ЦЕ: Кажемо екрану оновити ціну за кілька штук
                    OnPropertyChanged(nameof(TotalItemPrice));
                }
            }
        }
        public decimal TotalItemPrice => Price * Quantity > 0 ? Price * Quantity : Price;
        // Ці властивості керуватимуть тим, яку кнопку показувати!
        public bool IsInCart => Quantity > 0;
        public bool IsNotInCart => Quantity == 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}