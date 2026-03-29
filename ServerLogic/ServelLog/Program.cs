using System;
using ServelLog.Services;

namespace FoodHubServerLogic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var security = new SecurityManager();

            string randomEmail = $"newuser_{Guid.NewGuid().ToString().Substring(0, 4)}@mail.com";

            Console.WriteLine($"Test 1: Registering a completely new user ({randomEmail})");
            try
            {
                security.Register(randomEmail, "pass1234");
                Console.WriteLine("Result: User successfully registered\n");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[ERROR]: {ex.Message}\n");
            }

            Console.WriteLine("Test 2: Attempting to register an exist user (student@khnure.edu.ua)");
            try
            {
                security.Register("student@khnure.edu.ua", "qwerty1234");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[VALIDATION ERROR]: {ex.Message}\n");
                Console.ResetColor();
            }

            Console.WriteLine("Test 3: Attempting to register with an empty password");
            try
            {
                security.Register("hacker@mail.com", "");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[VALIDATION ERROR]: {ex.Message}\n");
                Console.ResetColor();
            }

            Console.WriteLine("Test 4: Authorizing the user");
            try
            {
                bool isLogged = security.Login("student@khnure.edu.ua", "qwerty1234");
                Console.WriteLine($"Result: Successful login? {isLogged}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Test 5: Authorizing with wrong password");
            try
            {
                bool isLogged = security.Login("student@khnure.edu.ua", "1111111");
                Console.WriteLine($"Result: Successful login? {isLogged}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("The work is complete, check the Data and Logs folders..");
        }
    }
}