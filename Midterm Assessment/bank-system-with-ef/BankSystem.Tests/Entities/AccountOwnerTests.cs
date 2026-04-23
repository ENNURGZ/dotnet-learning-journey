using System.Reflection;
using BankSystem.EF.Entities;
using NUnit.Framework;

namespace BankSystem.Tests.Entities;

[TestFixture]
public class AccountOwnerTests : ModelTestBase<AccountOwner>
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
            Is.EqualTo(5),
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
            Is.EqualTo(5),
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
            Is.EqualTo(10),
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

    [TestCase("account_owner")]
    public void HasTableAttribute(string tableName)
    {
        this.AssertThatHasTableAttribute(tableName);
    }

    [TestCase("Id", typeof(int), "account_owner_id")]
    [TestCase("FirstName", typeof(string), "first_name")]
    [TestCase("LastName", typeof(string), "last_name")]
    [TestCase("Email", typeof(string), "email")]
    public void HasProperty(string propertyName, Type propertyType, string columnName)
    {
        _ = this.AssertThatClassHasProperty(propertyName, propertyType, columnName);
    }

    [TestCase("Id")]
    public void HasKeyAttribute(string propertyName)
    {
        this.AssertThatPropertyHasKeyAttribute(propertyName);
    }
}
