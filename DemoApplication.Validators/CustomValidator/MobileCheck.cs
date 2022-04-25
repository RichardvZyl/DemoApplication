using System.Text.RegularExpressions;

namespace DemoApplication.Validator;

public static class StringExtensions
{
    /// <summary> Checks to be sure a phone number contains 10 or 11 digits as per requirement. </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static bool ValidatePhoneNumber(this string phone)
    {
        var numericOnly = phone.RemoveNonNumeric();
        var cleaned = phone.RemoveParenthesis();

        if (cleaned.StartsWith('+'))
        {
            if (numericOnly.Substring(0, 2) != "27") //first two numeric characters should be the country code
                return false;

            if (numericOnly.Length == 11) //ensures correct length
                return true;
        }

        return numericOnly.Length == 10 && numericOnly.StartsWith('0');
    }

    /// <summary> Removes all non numeric characters from a string </summary>
    /// <param name="phone"></param>
    /// <returns>Phone number with all non numeric characters removed</returns>
    public static string RemoveNonNumeric(this string phone)
    {
        phone = phone.Trim()
        .Replace(" ", "")
        .Replace("-", "")
        .Replace("(", "")
        .Replace(")", "");

        return Regex.Replace(phone, @"[^0-9]+", "");
    }

    /// <summary> Removes all parenthesis from a string </summary>
    /// <param name="phone"></param>
    /// <returns>Phone number without Parenthesis</returns>
    public static string RemoveParenthesis(this string phone) => phone.Trim()
        .Replace("(", "")
        .Replace(")", "");
}
