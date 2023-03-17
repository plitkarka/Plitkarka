using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Plitkarka.Core.Helpers;

public static class ValidationHelper
{
    public static bool IsNameValid(string name)
    {
        return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[A-Za-z]+$");
    }
    
    public static bool IsSurnameValid(string surname)
    {
        return !string.IsNullOrEmpty(surname) && Regex.IsMatch(surname, @"^[A-Za-z]+$");
    }

    public static bool IsEmailValid(string email)
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

    public static bool IsPasswordValid(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Length >= 8;
    }
    
    public static bool IsPasswordConfirmed(string password, string confirmPassword)
    {
        return password == confirmPassword;
    }

    public static bool IsBirthDateValid(DateTime birthDate)
    {
        var age = DateTime.Now.Year - birthDate.Year;
        
        if (age < 18)
        {
            return false;
        }
        else if (age == 18 && birthDate.Date > DateTime.Now.Date.AddYears(-age))
        {
            return false;
        }

        return true;
    }
}
