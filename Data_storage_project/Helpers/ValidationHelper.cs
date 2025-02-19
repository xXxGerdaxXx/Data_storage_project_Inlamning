using System;
using System.Linq;
using System.Net.Mail;

namespace Data_storage_project_library.Helpers;

public static class ValidationHelper
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(string phone)
    {
        return phone.All(char.IsDigit) && phone.Length >= 7 && phone.Length <= 15;
    }
}
