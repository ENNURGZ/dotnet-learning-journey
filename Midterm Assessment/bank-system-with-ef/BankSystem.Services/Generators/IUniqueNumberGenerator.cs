namespace BankSystem.Services.Generators;

/// <summary>
/// The IUniqueNumberGenerator interface outlines a contract for classes that aim to generate unique strings, used for unique identification bank account numbers.
/// </summary>
public interface IUniqueNumberGenerator
{
    /// <summary>
    /// Generate() method, when implemented, must generate a string that represents a unique number.
    /// </summary>
    /// <returns>A string that represents a unique number.</returns>
    string Generate();
}
