using ClientAppe.ViewModels;

namespace ClientAppe.Models
{
    public class FoodModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(); 
            }
        }
    }
}