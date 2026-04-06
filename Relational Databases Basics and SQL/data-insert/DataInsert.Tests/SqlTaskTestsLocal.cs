using System;
using System.IO;
using AutocodeDB.Helpers;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace SqlDataInsert.Tests
{
    [TestFixture]
    public class SqlTaskTestsLocal : SqlTaskTests
    {
        [Order(1)]
        [Test]
        public void FileWithQueriesExists()
        {
            var actual = File.Exists(QueriesFile);
            var message = $"Couldn't find the '{QueriesFileName}' file.";
            Assert.IsTrue(actual, message);
        }

        [Order(2)]
        [Test]
        public void FileWithQueriesNotEmpty()
        {
            var message = $"The file '{QueriesFileName}' contains no entries.";
            Assert.IsNotEmpty(Queries, message);
        }

        [Order(3)]
        [TestCaseSource(nameof(Queries))]
        public void InsertQueryStringContainsCorrectInsertInstruction(string query)
        {
            var actual = InsertHelper.ContainsCorrectInsertInstruction(query);
            var message = QueryHelper.ComposeErrorMessage(query, "The query doesn't contain 'INSERT INTO' instruction.");
            Assert.IsTrue(actual, message);
        }

        [Order(4)]
        [TestCaseSource(nameof(Queries))]
        public void AllInsertQueriesExecuteSuccessfully(string query)
        {
            try
            {
                var command = new SqliteCommand(query, SqliteHelper.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception e)
            {
                Assert.Fail(QueryHelper.ComposeErrorMessage(query, e, "Query execution caused an exception."));
            }
        }
    }
}