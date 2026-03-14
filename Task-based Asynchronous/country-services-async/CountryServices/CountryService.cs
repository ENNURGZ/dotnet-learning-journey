using System.Net;
using System.Text.Json;

namespace CountryServices;

/// <summary>
/// Provides information about country local currency from RESTful API
/// <see><cref>https://restcountries.com/#api-endpoints-v2</cref></see>.
/// </summary>
public class CountryService : ICountryService
{
    private const string ServiceUrl = "https://restcountries.com/v2";

    private static readonly HttpClient HttpClient = new HttpClient();

    private readonly Dictionary<string, WeakReference<LocalCurrency>> currencyCountries = new Dictionary<string, WeakReference<LocalCurrency>>();

    /// <summary>
    /// Gets information about currency by country code synchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
    public LocalCurrency GetLocalCurrencyByAlpha2Or3Code(string? alpha2Or3Code)
    {
        if (string.IsNullOrWhiteSpace(alpha2Or3Code))
        {
            throw new ArgumentException("Country code cannot be null or empty.", nameof(alpha2Or3Code));
        }

        try
        {
            using var webClient = new WebClient();
            var response = webClient.DownloadString($"{ServiceUrl}/alpha/{alpha2Or3Code}");

            var localCurrencyInfo = JsonSerializer.Deserialize<LocalCurrencyInfo>(response);
            if (localCurrencyInfo == null || localCurrencyInfo.Currencies == null || localCurrencyInfo.Currencies.Count == 0)
            {
                throw new ArgumentException("Invalid country code or currency not found.");
            }

            return new LocalCurrency
            {
                CountryName = localCurrencyInfo.CountryName,
                CurrencyCode = localCurrencyInfo.Currencies[0].Code,
                CurrencySymbol = localCurrencyInfo.Currencies[0].Symbol,
            };
        }
        catch (WebException)
        {
            throw new ArgumentException("Invalid country code or currency not found.", nameof(alpha2Or3Code));
        }
    }

    /// <summary>
    /// Gets information about currency by country code asynchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>.
    /// <param name="token">Token for cancellation asynchronous operation.</param>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
    public Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(string? alpha2Or3Code, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(alpha2Or3Code))
        {
            throw new ArgumentException("Country code cannot be null or empty.", nameof(alpha2Or3Code));
        }

        return GetLocalCurrencyByAlpha2Or3CodeAsyncInternal(alpha2Or3Code, token);
    }

    /// <summary>
    /// Gets information about the country by the country capital synchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <returns>Information about the country as <see cref="Country"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Country GetCountryInfoByCapital(string? capital)
    {
        if (string.IsNullOrWhiteSpace(capital))
        {
            throw new ArgumentException("Capital cannot be null or empty.", nameof(capital));
        }

        try
        {
            using var webClient = new WebClient();
            var response = webClient.DownloadString($"{ServiceUrl}/capital/{capital}");

            var countryInfos = JsonSerializer.Deserialize<CountryInfo[]>(response);
            if (countryInfos == null || countryInfos.Length == 0)
            {
                throw new ArgumentException("Invalid capital name.", nameof(capital));
            }

            var countryInfo = countryInfos[0];
            return new Country
            {
                Name = countryInfo.Name,
                CapitalName = countryInfo.CapitalName,
                Area = countryInfo.Area,
                Population = countryInfo.Population,
                Flag = countryInfo.Flag,
            };
        }
        catch (WebException)
        {
            throw new ArgumentException("Invalid capital name.", nameof(capital));
        }
    }

    /// <summary>
    /// Gets information about the currency by the country capital asynchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <param name="token">Token for cancellation asynchronous operation.</param>
    /// <returns>Information about the country as <see cref="Country"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Task<Country> GetCountryInfoByCapitalAsync(string? capital, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(capital))
        {
            throw new ArgumentException("Capital cannot be null or empty.", nameof(capital));
        }

        return GetCountryInfoByCapitalAsyncInternal(capital, token);
    }

    private static async Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsyncInternal(string alpha2Or3Code, CancellationToken token)
    {
        try
        {
            var response = await HttpClient.GetAsync(new Uri($"{ServiceUrl}/alpha/{alpha2Or3Code}"), token);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Invalid country code or currency not found.", nameof(alpha2Or3Code));
            }

            var content = await response.Content.ReadAsStringAsync(token);
            var localCurrencyInfo = JsonSerializer.Deserialize<LocalCurrencyInfo>(content);

            if (localCurrencyInfo == null || localCurrencyInfo.Currencies == null || localCurrencyInfo.Currencies.Count == 0)
            {
                throw new ArgumentException("Invalid country code or currency not found.");
            }

            return new LocalCurrency
            {
                CountryName = localCurrencyInfo.CountryName,
                CurrencyCode = localCurrencyInfo.Currencies[0].Code,
                CurrencySymbol = localCurrencyInfo.Currencies[0].Symbol,
            };
        }
        catch (HttpRequestException)
        {
            throw new ArgumentException("Invalid country code or currency not found.", nameof(alpha2Or3Code));
        }
    }

    private static async Task<Country> GetCountryInfoByCapitalAsyncInternal(string capital, CancellationToken token)
    {
        try
        {
            var response = await HttpClient.GetAsync(new Uri($"{ServiceUrl}/capital/{capital}"), token);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Invalid capital name.", nameof(capital));
            }

            var content = await response.Content.ReadAsStringAsync(token);
            var countryInfos = JsonSerializer.Deserialize<CountryInfo[]>(content);

            if (countryInfos == null || countryInfos.Length == 0)
            {
                throw new ArgumentException("Invalid capital name.", nameof(capital));
            }

            var countryInfo = countryInfos[0];
            return new Country
            {
                Name = countryInfo.Name,
                CapitalName = countryInfo.CapitalName,
                Area = countryInfo.Area,
                Population = countryInfo.Population,
                Flag = countryInfo.Flag,
            };
        }
        catch (HttpRequestException)
        {
            throw new ArgumentException("Invalid capital name.", nameof(capital));
        }
    }
}
