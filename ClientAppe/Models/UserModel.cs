namespace ClientAppe.Models
{
    public class UserModel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        // Додаємо поля для особистого кабінету
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RegistrationDate { get; set; }
        public string Address { get; set; }
    }
}