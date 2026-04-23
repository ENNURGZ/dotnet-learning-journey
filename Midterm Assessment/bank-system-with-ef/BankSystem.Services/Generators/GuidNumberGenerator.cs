using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

/// <summary>
/// GuidGenerator class provides a concrete implementation for the IUniqueNumberGenerator interface. This class generates a unique string based on a globally unique identifier (GUID) with the help of the CryptoHelper class.
/// </summary>
public sealed class GuidNumberGenerator : IUniqueNumberGenerator
{
    /// <summary>
    /// Generate() method provides a concrete implementation for the IUniqueNumberGenerator interface.
    /// </summary>
    /// <returns>A unique string based on a GUID.</returns>
    public string Generate()
    {
        return Guid.NewGuid().ToString().GenerateHash();
    }
}
