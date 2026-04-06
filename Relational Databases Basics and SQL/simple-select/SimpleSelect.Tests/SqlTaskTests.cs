using System;
using System.IO;
using AutocodeDB.Helpers;
using AutocodeDB.Models;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace SimpleSelect.Tests
{
    [TestFixture]
    public class SqlTaskTests
    {
        private const int FilesCount = 5;
        private readonly string databaseFile;
        private readonly string emptyDatabaseFile;
        private readonly string insertFile;
        private readonly string[] fileNames;
        private readonly string[] queryFiles;
        private readonly string[] queriesArray;
        private SelectResult[] actualResults;
        private SelectResult[] expectedResults;

        public SqlTaskTests()
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
            // This region can be modified if you will update xtask data or description.
            // Comment this out of the skeleton creation.
            SqliteHelper.OpenConnection(this.databaseFile);

            // If you are going update data for sceleton uncoment next 3 lines of code.
            // SqliteHelper.OpenConnection(this.emptyDatabaseFile, this.databaseFile);
            // InsertData();
            // FileIOHelper.SerializeResultFiles(this.actualResults,fileNames);
            this.expectedResults = FileIOHelper.DeserializeResultFiles(this.fileNames);
            this.actualResults = SelectHelper.GetResults(this.queriesArray);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            SqliteHelper.CloseConnection();
        }

        [Test]
        public void FileWithQueriesExists([Range(1, FilesCount)] int index)
        {
            this.AssertFileExist(index - 1);
        }

        [Test]
        public void FileWithQueriesNotEmpty([Range(1, FilesCount)] int index)
        {
            this.AssertFileNotEmpty(index - 1);
        }

        [Test]
        public void AllInsertQueriesExecuteSuccessfully([Range(1, FilesCount)] int index)
        {
            this.AssertData(index - 1);
        }

        [Test]
        public void SelectQueryReturnsCorrectRowsCount([Range(1, FilesCount)] int index)
        {
            index -= 1;
            this.AssertData(index);
            var expected = this.expectedResults[index].Data.Length;
            var actual = this.actualResults[index].Data.Length;
            Assert.AreEqual(expected, actual, this.actualResults[index].ErrorMessage);
        }

        [Test]
        public void SelectQueryReturnsCorrectSchema([Range(1, FilesCount)] int index)
        {
            index -= 1;
            this.AssertData(index);
            var expected = this.expectedResults[index].Schema;
            var actual = this.actualResults[index].Schema;
            var expectedMessage = MessageComposer.Compose(expected);
            var actualMessage = MessageComposer.Compose(actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        [Test]
        public void SelectQueryReturnsCorrectTypes([Range(1, FilesCount)] int index)
        {
            index -= 1;
            this.AssertData(index);
            var expected = this.expectedResults[index].Types;
            var actual = this.actualResults[index].Types;
            var expectedMessage = MessageComposer.Compose(expected);
            var actualMessage = MessageComposer.Compose(actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        [Test]
        public void SelectQueryReturnsCorrectData([Range(1, FilesCount)] int index)
        {
            index -= 1;
            this.AssertData(index);
            var expected = this.expectedResults[index].Data;
            var actual = this.actualResults[index].Data;
            var expectedMessage = MessageComposer.Compose(this.expectedResults[index].Schema, expected);
            var actualMessage = MessageComposer.Compose(this.actualResults[index].Schema, actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        [Test]
        public void SelectQueryContainsSelectFrom([Range(1, FilesCount)] int index)
        {
            var actual = this.queriesArray[index - 1];
            Assert.IsTrue(SelectHelper.ContainsSelectFrom(actual), "Query should contain 'SELECT' and 'FROM' statements.");
        }

        [Test]
        public void SelectQueryContainsOrderBy([Range(1, FilesCount)] int index)
        {
            var actual = this.queriesArray[index - 1];
            Assert.IsTrue(SelectHelper.ContainsOrderBy(actual), "Query should contain 'ORDER BY' statement.");
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

        private void InsertData()
        {
            var queries = QueryHelper.GetQueries(this.insertFile);
            foreach (var query in queries)
            {
                var command = new SqliteCommand(query, SqliteHelper.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }
    }
}