using System.Text.RegularExpressions;
using BankSystem.Services.Generators;
using NUnit.Framework;

namespace BankSystem.Tests.NumberGeneratorTests;

[TestFixture]
public sealed class UniqueNumberGenerator
{
    public static IEnumerable<IUniqueNumberGenerator> GeneratorTestCases
    {
        get
        {
            yield return new GuidNumberGenerator();
            yield return SimpleGenerator.Instance;
            yield return new BasedOnTickUniqueNumberGenerator(new DateTime(2022, 2, 2));
        }
    }

    [TestCaseSource(nameof(GeneratorTestCases))]
    public void Generate_ReturnUniqueValues(IUniqueNumberGenerator generator)
    {
        string first = generator.Generate();
        string second = generator.Generate();
        Assert.That(first != second);
    }

    [TestCaseSource(nameof(GeneratorTestCases))]
    public void Generate_ReturnStringOfLength32(IUniqueNumberGenerator generator)
        => Assert.That(32, Is.EqualTo(generator.Generate().Length));

    [TestCaseSource(nameof(GeneratorTestCases))]
    public void Generate_ShouldReturnStringContainingOnlyHexadecimalCharacters(IUniqueNumberGenerator generator)
        => Assert.That(Regex.IsMatch(generator.Generate(), "^[a-zA-Z0-9]*$"));
}
