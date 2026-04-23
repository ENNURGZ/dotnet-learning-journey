using System.Reflection;
using BankSystem.EF.Entities;
using NUnit.Framework;

namespace BankSystem.Tests.Entities;

[TestFixture]
public class BankAccountTests : ModelTestBase<BankAccount>
{
    [Test]
    public void IsPublicClass()
    {
        this.AssertThatClassIsPublic(false);
    }

    [Test]
    public void InheritsObject()
    {
        this.AssertThatClassInheritsObject();
    }

    [Test]
    public void HasRequiredMembers()
    {
        Assert.That(
            this.ClassType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length,
            Is.EqualTo(0),
            "Checking fields number");
        Assert.That(
            this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.Public).Length,
            Is.EqualTo(0),
            "Checking fields number");
        Assert.That(
            this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length,
            Is.EqualTo(9),
            "Checking fields number");

        Assert.That(
            this.ClassType.GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length,
            Is.EqualTo(0),
            "Checking constructor number");
        Assert.That(
            this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length,
            Is.EqualTo(1),
            "Checking constructor number");
        Assert.That(
            this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Length,
            Is.EqualTo(0),
            "Checking constructor number");

        Assert.That(
            this.ClassType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length,
            Is.EqualTo(0),
            "Checking properties number");
        Assert.That(
            this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length,
            Is.EqualTo(9),
            "Checking properties number");
        Assert.That(
            this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Length,
            Is.EqualTo(0),
            "Checking properties number");

        Assert.That(
            this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Length,
            Is.EqualTo(0),
            "Checking methods number");
        Assert.That(
            this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length,
            Is.EqualTo(0),
            "Checking methods number");

        Assert.That(
            this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length,
            Is.EqualTo(18),
            "Checking methods number");
        Assert.That(
            this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Length,
            Is.EqualTo(0),
            "Checking methods number");

        Assert.That(
            this.ClassType.GetEvents(
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length,
            Is.EqualTo(0),
            "Checking events number");
    }

    [TestCase("bank_account")]
    public void HasTableAttribute(string tableName)
    {
        this.AssertThatHasTableAttribute(tableName);
    }

    [TestCase("Id", typeof(int), "bank_account_id")]
    [TestCase("AccountOwnerId", typeof(int), "account_owner_id")]
    [TestCase("Number", typeof(string), "account_number")]
    [TestCase("Balance", typeof(decimal), "balance")]
    [TestCase("CurrencyCodeId", typeof(int), "currency_code_id")]
    [TestCase("BonusPoints", typeof(int), "bonus_points")]
    [TestCase("Overdraft", typeof(decimal), "overdraft")]
    [TestCase("AccountOwner", typeof(AccountOwner), null)]
    [TestCase("CurrencyCode", typeof(CurrencyCode), null)]
    public void HasProperty(string propertyName, Type propertyType, string columnName)
    {
        _ = this.AssertThatClassHasProperty(propertyName, propertyType, columnName);
    }

    [TestCase("Id")]
    public void HasKeyAttribute(string propertyName)
    {
        this.AssertThatPropertyHasKeyAttribute(propertyName);
    }

    [TestCase("AccountOwnerId", "AccountOwner")]
    [TestCase("CurrencyCodeId", "CurrencyCode")]
    public void HasForeignKeyAttribute(string propertyName, string navigationPropertyName)
    {
        this.AssertThatPropertyHasForeignKeyAttribute(propertyName, navigationPropertyName);
    }
}
