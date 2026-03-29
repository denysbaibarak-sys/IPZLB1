using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using ServelLog.Models;

namespace ServelLog.Services
{
    public class SecurityManager
    {
        private readonly string _dataFolder = "Data";
        private readonly string _logsFolder = "Logs";
        private readonly string _usersFilePath;
        private readonly string _logFilePath;

        public SecurityManager()
        {
            if (!Directory.Exists(_dataFolder)) Directory.CreateDirectory(_dataFolder);
            if (!Directory.Exists(_logsFolder)) Directory.CreateDirectory(_logsFolder);

            _usersFilePath = Path.Combine(_dataFolder, "users.json");
            _logFilePath = Path.Combine(_logsFolder, "server.log");
        }

        public void LogAction(string message)
        {
            using (var file = new StreamWriter(_logFilePath, true))
            {
                file.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {message}");
            }
        }

        private void ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                LogAction("Validation error: Attempting to pass empty client data.");
                throw new ArgumentException("Email and password cannot be empty!");
            }
        }
        // Заглушка для загрузки и сохранения пользователей
        private List<User> LoadUsers()
        {
            if (!File.Exists(_usersFilePath))
            {
                return new List<User>();
            }

            var jsonSerializer = new DataContractJsonSerializer(typeof(List<User>));
            using (var file = new FileStream(_usersFilePath, FileMode.Open))
            {
                if (file.Length == 0) return new List<User>();
                var users = jsonSerializer.ReadObject(file) as List<User>;
                return users ?? new List<User>();
            }
        }

        private void SaveUsers(List<User> users)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<User>));
            using (var file = new FileStream(_usersFilePath, FileMode.Create))
            {
                jsonSerializer.WriteObject(file, users);
            }
        }

        public bool Register(string email, string password)
        {
            ValidateCredentials(email, password);

            var users = LoadUsers();

            if (users.Any(u => u.Email == email))
            {
                LogAction($"Error: Attempting to register an existing customer {email}");
                throw new ArgumentException($"User with email '{email}' already exists!");
            }

            users.Add(new User { Email = email, Password = password });
            SaveUsers(users);

            LogAction($"Successful new client registration: {email}");
            return true;
        }

        public bool Login(string email, string password)
        {
            ValidateCredentials(email, password);

            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                LogAction($"Successful client authorization: {email}");
                return true;
            }

            LogAction($"Failed login attempt for client: {email}");
            return false;
        }
    }
}
