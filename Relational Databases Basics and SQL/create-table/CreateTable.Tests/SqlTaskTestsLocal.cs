using System;
using System.Collections.Generic;
using System.IO;
using AutocodeDB.Helpers;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace SqlCreateTable.Tests
{
    [TestFixture]
    public class SqlTaskTestsLocal : SqlTaskTests
    {
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
    }
}
