using System.Globalization;
using System.Text.RegularExpressions;

namespace BankSystem.Services.Helpers;

/// <summary>
/// The ValidatorService class is a static helper class that provides methods for validation needs. It currently supports validation of currencies and emails.
/// </summary>
public static class ValidationService
{
    /// <summary>
    /// IsCurrencyValid extension method checks if the specified currency is valid. It makes use of the CultureInfo and RegionInfo classes to match the currency against a list of known currencies.
    /// </summary>
    /// <param name="currencyCode">The currency code to validate.</param>
    /// <returns>True if the currency code is valid; otherwise, false.</returns>
    public static bool IsCurrencyValid(this string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return false;
        }

        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        foreach (var culture in cultures)
        {
            try
            {
                var region = new RegionInfo(culture.Name);
                if (string.Equals(region.ISOCurrencySymbol, currencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            catch (ArgumentException)
            {
                // Some cultures might not have a region
            }
        }

        return false;
    }

    /// <summary>
    /// IsEmailValid extension method verifies that a specified email is in a correct format.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>True if the email is valid; otherwise, false.</returns>
    public static bool IsEmailValid(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        // Basic regex for email validation
        const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}
