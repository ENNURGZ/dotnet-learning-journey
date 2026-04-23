using System.Reflection;
using BankSystem.Services.Generators;
using BankSystem.Services.Models;
using BankSystem.Services.Models.Accounts;
using Moq;
using NUnit.Framework;

namespace BankSystem.Tests.Models;

[TestFixture]
public sealed class AccountOwnerTests
{
    private static IEnumerable<(string firstName, string lastName, string email)> TestCasesForPropertiesVerification
    {
        get
        {
            yield return ("Arlene", "Robel", "Arlene.Robel96@hotmail.com");
            yield return ("Clement", "Fisher", "Clement.Fisher@gmail.com");
            yield return ("Danielle", "McGlynn", "Danielle83@gmail.com");
            yield return ("Garrett", "Jast", "Garrett21@gmail.com");
            yield return ("Garrick", "Little", "Garrick_Little@gmail.com");
            yield return ("Jared", "Harber", "Jared.Harber48@hotmail.com");
            yield return ("Jaycee", "Rempel", "Jaycee.Rempel57@yahoo.com");
            yield return ("Lily", "Schneider", "Lily_Schneider83@gmail.com");
            yield return ("Madisen", "Ondricka", "Madisen_Ondricka6@hotmail.com");
            yield return ("Nicole", "Schmitt", "Nicole58@gmail.com");
        }
    }

    private static IEnumerable<(AccountOwner owner, string expected)> TestCasesForToString
    {
        get
        {
            yield return (new AccountOwner("Arlene", "Robel", "Arlene.Robel96@hotmail.com"), "Arlene Robel, Arlene.Robel96@hotmail.com.");
            yield return (new AccountOwner("Clement", "Fisher", "Clement.Fisher@gmail.com"), "Clement Fisher, Clement.Fisher@gmail.com.");
            yield return (new AccountOwner("Danielle", "McGlynn", "Danielle83@gmail.com"), "Danielle McGlynn, Danielle83@gmail.com.");
            yield return (new AccountOwner("Garrett", "Jast", "Garrett21@gmail.com"), "Garrett Jast, Garrett21@gmail.com.");
            yield return (new AccountOwner("Garrick", "Little", "Garrick_Little@gmail.com"), "Garrick Little, Garrick_Little@gmail.com.");
            yield return (new AccountOwner("Jared", "Harber", "Jared.Harber48@hotmail.com"), "Jared Harber, Jared.Harber48@hotmail.com.");
            yield return (new AccountOwner("Jaycee", "Rempel", "Jaycee.Rempel57@yahoo.com"), "Jaycee Rempel, Jaycee.Rempel57@yahoo.com.");
            yield return (new AccountOwner("Lily", "Schneider", "Lily_Schneider83@gmail.com"), "Lily Schneider, Lily_Schneider83@gmail.com.");
            yield return (new AccountOwner("Madisen", "Ondricka", "Madisen_Ondricka6@hotmail.com"), "Madisen Ondricka, Madisen_Ondricka6@hotmail.com.");
            yield return (new AccountOwner("Nicole", "Schmitt", "Nicole58@gmail.com"), "Nicole Schmitt, Nicole58@gmail.com.");
        }
    }

    [TestCaseSource(nameof(TestCasesForPropertiesVerification))]
    public void Constructor_Should_Initialize_Properties((string firstName, string lastName, string email) value)
    {
        var owner = new AccountOwner(value.firstName, value.lastName, value.email);

        Assert.That(value, Is.EqualTo((owner.FirstName, owner.LastName, owner.Email)));
    }

    [TestCaseSource(nameof(TestCasesForToString))]
    public void ToString_ShouldReturnCorrectString((AccountOwner owner, string expected) value)
    {
        Assert.That(value.owner.ToString(), Is.EqualTo(value.expected));
    }

    [Test]
    public void Add_New_Account_To_Accounts()
    {
        var owner = new AccountOwner("John", "Doe", "john.doe@gmail.com");
        var mockGenerator = new Mock<IUniqueNumberGenerator>();
        _ = mockGenerator.Setup(m => m.Generate()).Returns("1234567890");
        var mockAccount = new Mock<BankAccount>(owner, "USD", mockGenerator.Object, 1000m);
        BankAccount account = mockAccount.Object;
        int countBefore = owner.Accounts().Count;
        owner.Add(account);
        int countAfter = owner.Accounts().Count;
        Assert.That(countBefore == countAfter - 1);
        Assert.That(owner.Accounts().Contains(account));
    }

    [Test]
    public void Constructor_InvalidEmail_ThrowArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => _ = new AccountOwner("John", "Doe", "invalid email"));
    }

    [Test]
    public void Constructor_EmptyFirstName_ThrowArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => _ = new AccountOwner(string.Empty, "Doe", "john.doe@gmail.com"));
    }

    [Test]
    public void Constructor_EmptyLastName_ThrowArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => _ = new AccountOwner("John", string.Empty, "john.doe@gmail.com"));
    }

    [Test]
    public void VerifyString_EmptyString_ThrowArgumentException()
    {
        var methodInfo = typeof(AccountOwner).GetMethod("VerifyString", BindingFlags.NonPublic | BindingFlags.Static);
        try
        {
            _ = methodInfo!.Invoke(null, [string.Empty, "paramName"]);
        }
        catch (TargetInvocationException ex)
        {
            Assert.That(ex.InnerException is ArgumentException);
        }
    }
}
