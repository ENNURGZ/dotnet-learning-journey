using System;
using System.IO;
using AutocodeDB.Helpers;
using AutocodeDB.Models;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace AlterTable.Tests
{
    [TestFixture]
    public class AlterTableTasksTests
    {
        private const int FilesCount = 3;
        private static readonly string[] DMLTargetTables = { "manufacturer", "person", "supermarket" };
        private readonly string databaseFile;
        private readonly string emptyDatabaseFile;
        private readonly string insertFile;
        private readonly string[] fileNames;
        private readonly string[] queryFiles;
        private readonly string[] queriesArray;
        private SelectResult[] actualResults;
        private SelectResult[] expectedResults;

        public AlterTableTasksTests()
        {
            FileIOHelper.FilesCount = FilesCount;
            FileIOHelper.GenerateProjectDirectory(Environment.CurrentDirectory);
            this.databaseFile = FileIOHelper.GetDBFullPath("marketplace.db");
            this.emptyDatabaseFile = FileIOHelper.GetEmptyDBFullPath("empty_tables.db");
            this.insertFile = FileIOHelper.GetInsertFileFullPath("insert.sql");
            this.fileNames = FileIOHelper.GetFilesNames();
            this.queryFiles = SqlTask.GetFilePaths(this.fileNames);
            this.queriesArray = QueryHelper.GetQueries(this.queryFiles);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            this.actualResults = new SelectResult[FilesCount];
            this.expectedResults = new SelectResult[FilesCount];

            this.expectedResults = FileIOHelper.DeserializeResultFiles(this.fileNames);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
        }

        [SetUp]
        public void LocalSetup()
        {
            SqliteHelper.OpenConnection(this.databaseFile);
        }

        [TearDown]
        public void LocalCleanup()
        {
            SqliteHelper.CloseConnection();
        }

        [Order(1)]
        [Test]
        public void FileWithQueriesExists([Range(1, FilesCount)] int index)
        {
            this.AssertFileExist(index - 1);
        }

        [Order(2)]
        [Test]
        public void FileWithQueriesNotEmpty([Range(1, FilesCount)] int index)
        {
            this.AssertFileNotEmpty(index - 1);
        }

        [Order(3)]
        [Test]
        public void AlterQueryContainsAlterTableAddColumn([Range(1, FilesCount)] int index)
        {
            --index;
            var actual = this.queriesArray[index];
            Assert.IsTrue(AlterTableHelper.ContainsAddColumn(actual), "Query should contain correct ALTER TABLE ADD COLUMN statement.");
        }

        [Order(4)]
        [Test]
        public void AllAlterQueriesExecuteSuccessfully([Range(1, FilesCount)] int index)
        {
            --index;
            var actual = this.queriesArray[index];
            Assert.False(string.IsNullOrEmpty(actual), "Query can not be empty");
            try
            {
                var command = new SqliteCommand(actual, SqliteHelper.Connection);
                command.ExecuteNonQuery();
                this.actualResults[index] = SelectHelper.DumpTable(DMLTargetTables[index]);
                command.Dispose();
            }
            catch (SqliteException exception)
            {
                var message = QueryHelper.ComposeErrorMessage(actual, exception, "Query execution caused an exception.");
                Assert.Fail(message);
            }
        }

        [Order(5)]
        [Test]
        public void AlterQueryMakesCorrectSchema([Range(1, FilesCount)] int index)
        {
            --index;
            this.AssertData(index);
            var expected = this.expectedResults[index].Schema;
            var actual = this.actualResults[index].Schema;
            var expectedMessage = MessageComposer.Compose(expected);
            var actualMessage = MessageComposer.Compose(actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        [Order(6)]
        [Test]
        public void AlterQueryMakesCorrectTypes([Range(1, FilesCount)] int index)
        {
            --index;
            this.AssertData(index);
            var expected = this.expectedResults[index].Types;
            var actual = this.actualResults[index].Types;
            var expectedMessage = MessageComposer.Compose(expected);
            var actualMessage = MessageComposer.Compose(actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        [Order(7)]
        [Test]
        public void AlterQueryMakesCorrectData([Range(1, FilesCount)] int index)
        {
            Console.WriteLine(this.fileNames);
            --index;
            this.AssertData(index);
            Console.WriteLine(this.expectedResults[index]);
            var expected = this.expectedResults[index].Data;
            var actual = this.actualResults[index].Data;
            var expectedMessage = MessageComposer.Compose(this.expectedResults[index].Schema, expected);
            var actualMessage = MessageComposer.Compose(this.actualResults[index].Schema, actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        private void AssertData(int index)
        {
            this.AssertFileExist(index);
            this.AssertFileNotEmpty(index);
            this.AssertErrors(index);
        }

        private void AssertErrors(int index)
        {
            if (!string.IsNullOrEmpty(this.actualResults[index].ErrorMessage))
            {
                Assert.Fail(this.actualResults[index].ErrorMessage);
            }
        }

        private void AssertFileExist(int index)
        {
            var actual = this.queriesArray[index];
            var message = $"The file '{this.fileNames[index]}' was not found.";
            if (actual == null)
            {
                Assert.Fail(message);
            }
        }

        private void AssertFileNotEmpty(int index)
        {
            var actual = this.queriesArray[index];
            var message = $"The file '{this.fileNames[index]}' contains no entries.";
            if (string.IsNullOrWhiteSpace(actual))
            {
                Assert.Fail(message);
            }
        }
    }
}