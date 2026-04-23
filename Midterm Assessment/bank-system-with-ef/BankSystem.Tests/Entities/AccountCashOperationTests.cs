using System.Reflection;
using BankSystem.EF.Entities;
using NUnit.Framework;

namespace BankSystem.Tests.Entities;

[TestFixture]
public class AccountCashOperationTests : ModelTestBase<AccountCashOperation>
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
            Is.EqualTo(6),
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
            Is.EqualTo(12),
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

    [TestCase("account_cash_operation")]
    public void HasTableAttribute(string tableName)
    {
        this.AssertThatHasTableAttribute(tableName);
    }

    [TestCase("Id", typeof(int), "account_cash_operation_id")]
    [TestCase("BankAccountId", typeof(int), "bank_account_id")]
    [TestCase("Amount", typeof(decimal), "amount")]
    [TestCase("OperationDateTime", typeof(string), "operation_date_time")]
    [TestCase("Note", typeof(string), "note")]
    [TestCase("BankAccount", typeof(BankAccount), null)]
    public void HasProperty(string propertyName, Type propertyType, string columnName)
    {
        _ = this.AssertThatClassHasProperty(propertyName, propertyType, columnName);
    }

    [TestCase("Id")]
    public void HasKeyAttribute(string propertyName)
    {
        this.AssertThatPropertyHasKeyAttribute(propertyName);
    }

    [TestCase("BankAccountId", "BankAccount")]
    public void HasForeignKeyAttribute(string propertyName, string navigationPropertyName)
    {
        this.AssertThatPropertyHasForeignKeyAttribute(propertyName, navigationPropertyName);
    }
}
