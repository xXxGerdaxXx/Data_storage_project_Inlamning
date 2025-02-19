using System;
using System.Text.RegularExpressions;

namespace Data_storage_project_library.Helpers
{
    public static partial class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$"); 
        }

        public static string GetValidatedInput(string prompt, bool required = true)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (required && string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: This field is required. Please enter a value.");
                }

            } while (required && string.IsNullOrWhiteSpace(input));

            return input;
        }
    }
}
