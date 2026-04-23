using BankSystem.Services.Generators;
using NUnit.Framework;

namespace BankSystem.Tests.NumberGeneratorTests;

[TestFixture]
public sealed class SimpleGeneratorTests
{
    [Test]
    public void Instance_ShouldAlwaysReturnSameInstance()
    {
        var instance1 = SimpleGenerator.Instance;
        var instance2 = SimpleGenerator.Instance;

        Assert.That(instance1 == instance2);
    }
}
