using System;
using System.IO;
using AutocodeDB.Helpers;
using AutocodeDB.Models;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace SqlCreateTable.Tests
{
    [TestFixture]
    public class SqlTaskTests
    {
        private const int CreateQueriesCount = 15;
        private const string CreateQueriesFileName = "create.sql";
        private static readonly string CreateQueriesFile = SqlTask.GetQueriesFullPath(CreateQueriesFileName);
        private static readonly string[] CreateQueries = QueryHelper.GetQueries(CreateQueriesFile);
        private static readonly IEnumerable<string> CreateQueriesWithForeignKeys =
            CreateTableHelper.GetOnlyQueriesWithForeignKeys(CreateQueries);

        [OneTimeSetUp]
        public void Setup()
        {
            SqliteHelper.OpenConnection();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            SqliteHelper.CloseConnection();
        }

        #region Local Tests

        [Test]
        public void FileWithQueriesExists()
        {
            Console.WriteLine($"path={CreateQueriesFile}");
            var actual = File.Exists(CreateQueriesFile);
            var message = $"Couldn't find the '{CreateQueriesFileName}' file.";
            Assert.IsTrue(actual, message);
        }

        [Test]
        public void FileWithQueriesNotEmpty()
        {
            var message = $"The file '{CreateQueriesFileName}' contains no entries.";
            Assert.IsNotEmpty(CreateQueries, message);
        }

        [Test]
        public void FileWithQueriesQueriesCount()
        {
            var message = $"There should be at least {CreateQueriesCount} queries in the '{CreateQueriesFileName}' file.";
            Assert.GreaterOrEqual(CreateQueries.Length, CreateQueriesCount, message);
        }

        [TestCaseSource(nameof(CreateQueries))]
        public void CreateTableQueryStringContainsCorrectCreateTableStatement(string query)
        {
            var actual = CreateTableHelper.ContainsCreateTableStatement(query);
            var message = QueryHelper.ComposeErrorMessage(query, "The query doesn't contain 'CREATE TABLE' statement.");
            Assert.IsTrue(actual, message);
        }

        [TestCaseSource(nameof(CreateQueries))]
        public void AllCreateTableQueriesExecutesSuccessfully(string query)
        {
            try
            {
                var command = new SqliteCommand(query, SqliteHelper.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (SqliteException exception)
            {
                var message = QueryHelper.ComposeErrorMessage(query, exception, "Query execution caused an exception.");
                Assert.Fail(message);
            }
        }

        [TestCaseSource(nameof(CreateQueriesWithForeignKeys))]
        public void CreateQueriesWithForeignKeysForeignKeyReferencesExist(string query)
        {
            try
            {
                CreateTableHelper.ValidateConstrainKeyIntegrity(query, CreateQueries);
            }
            catch (Exception exception)
            {
                var message = QueryHelper.ComposeErrorMessage(query, exception);
                Assert.Fail(message);
            }
        }

        [TestCaseSource(nameof(CreateQueriesWithForeignKeys))]
        public void CreateQueriesWithForeignKeysOnDeleteCascadeNotExist(string query)
        {
            var message = QueryHelper.ComposeErrorMessage(query, "Each table should not contain ON DELETE CASCADE.");
            var actual = CreateTableHelper.ContainsOnDeleteCascade(query);
            Assert.IsFalse(actual, message);
        }
        #endregion

        #region Autocode Tests

        [TestCaseSource(nameof(CreateQueries))]
        public void CreateTableQueryStringAllTablesContainsPrimaryKey(string query)
        {
            var actual = CreateTableHelper.ContainsPrimaryKey(query);
            var message = QueryHelper.ComposeErrorMessage(query, "Each table should contain a Primary key.");
            Assert.IsTrue(actual, message);
        }

        [Test]
        public void CreateTableQueryStringContainsAtListOneForeignKey()
        {
            var actual = QueryHelper.IsQueryCorrect(CreateQueries, CreateTableHelper.ContainsForeignKey);
            const string message = "Database should contain at least one Foreign key entry.";
            Assert.IsTrue(actual, message);
        }

        [Test]
        public void CreateTableQueryStringContainsAtListOneUniqueKey()
        {
            var actual = QueryHelper.IsQueryCorrect(CreateQueries, CreateTableHelper.ContainsUniqueKey);
            const string message = "Database should contain at least one Unique key entry.";
            Assert.IsTrue(actual, message);
        }

        [TestCase(1, 4)]
        [TestCase(2, 4)]
        [TestCase(4, 1)]
        public void CreateTableForeingKeyRequirment(int numberFK, int expectedTables)
        {
            var actual = CreateTableHelper.TablesWithForeignKeys(numberFK);
            string message = $"Database should contain at least {expectedTables} tables with {numberFK} Foreign key(s) entry.";
            Assert.IsTrue(actual >= expectedTables, message);
        }

        #endregion
    }
}
