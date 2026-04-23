using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

/// <summary>
/// The SimpleGenerator class serves as a simple unique number generator that generates sequential numbers. This class implements the IUniqueNumberGenerator interface and adheres to the Singleton design pattern.
/// </summary>
public sealed class SimpleGenerator : IUniqueNumberGenerator
{
    private int lastNumber = 1234567890;

    private static readonly SimpleGenerator instance = new SimpleGenerator();

    /// <summary>
    /// Prevents a default instance of the <see cref="SimpleGenerator"/> class from being created.
    /// </summary>
    private SimpleGenerator()
    {
    }

    /// <summary>
    /// Gets the instance of SimpleGenerator.
    /// </summary>
    public static SimpleGenerator Instance => instance;

    /// <summary>
    /// Generate() method generates and returns a unique sequential number starting from previously generated number. Each newly generated number is hashed using the MD5 hashing algorithm for additional security and returned as a string.
    /// </summary>
    /// <returns>A hashed string of the sequential number.</returns>
    public string Generate()
    {
        lastNumber++;
        return lastNumber.ToString(System.Globalization.CultureInfo.InvariantCulture).GenerateHash();
    }
}
