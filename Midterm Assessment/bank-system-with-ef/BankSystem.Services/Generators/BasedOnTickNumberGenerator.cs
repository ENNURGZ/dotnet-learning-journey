using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

/// <summary>
/// BasedOnTickUniqueNumberGenerator class calculates the elapsed ticks (time) from the startingPoint to the current time, hashes this value and returns it as a string.
/// </summary>
public sealed class BasedOnTickUniqueNumberGenerator : IUniqueNumberGenerator
{
    private readonly DateTime startingPoint;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasedOnTickUniqueNumberGenerator"/> class.
    /// </summary>
    /// <param name="startingPoint">The starting point in time from which the ticks (time) would be counted.</param>
    public BasedOnTickUniqueNumberGenerator(DateTime startingPoint)
    {
        this.startingPoint = startingPoint;
    }

    /// <summary>
    /// Generate() method calculates the elapsed ticks (time) from the startingPoint to the current time, hashes this value and returns it as a string.
    /// </summary>
    /// <returns>A hashed string of the elapsed ticks.</returns>
    public string Generate()
    {
        long ticks = (DateTime.Now - this.startingPoint).Ticks;
        return ticks.ToString(System.Globalization.CultureInfo.InvariantCulture).GenerateHash();
    }
}
