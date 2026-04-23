using System.Globalization;
using BankSystem.Tests.AutocodeDB.Helpers;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace BankSystem.Tests.Entities;

[TestFixture]
public sealed class BankContextTests : IDisposable
{
    private readonly string databaseFile = FileIOHelper.GetDBFullPath("bank-system.db");
    private SqliteConnection connection = null!;

    [SetUp]
    public void SetUp()
    {
        var connectionString = FileConnectionString(this.databaseFile, readOnly: true);
        this.connection = new SqliteConnection(connectionString);
        this.connection.Open();
    }

    [TestCase("account_owner", ExpectedResult = 3)]
    [TestCase("currency_code", ExpectedResult = 20)]
    [TestCase("bank_account", ExpectedResult = 5)]
    [TestCase("account_cash_operation", ExpectedResult = 15)]
    public int TestDbSet_ReturnsCount(string dbSetName)
    {
        // Act
        Assert.That(dbSetName, Is.Not.Null);
        int count = this.CountRows(dbSetName);
        return count;
    }

    public void Dispose()
    {
        this.connection.Dispose();
    }

    public int CountRows(string tableName)
    {
        var countCmd = new SqliteCommand($"SELECT COUNT(*) FROM {tableName}", this.connection);
        var res = countCmd.ExecuteScalar();
        countCmd.Dispose();
        return Convert.ToInt32(res, CultureInfo.InvariantCulture);
    }

    private static string FileConnectionString(string fileName, bool readOnly = false) => new SqliteConnectionStringBuilder
    {
        DataSource = fileName,
        Mode = readOnly ? SqliteOpenMode.ReadOnly : SqliteOpenMode.ReadWriteCreate,
    }.ConnectionString;
}
