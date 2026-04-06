using System;
using System.Collections.Generic;
using System.IO;
using AutocodeDB.Helpers;
using AutocodeDB.Models;
using CorrelatedSubqueries;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace SqlCreateTable.Tests
{
    [TestFixture]
    public class SqlTaskGPTTests
    {
        private const int FilesCount = 4;
        private readonly string[] gptFileNames;
        private readonly string[] gptFiles;

        public SqlTaskGPTTests()
        {
            FileIOHelper.FilesCount = FilesCount;
            FileIOHelper.GenerateProjectDirectory(Environment.CurrentDirectory);
            this.gptFileNames = FileIOHelper.GetFilesNames(FilesCount, "gpt-stage-", "txt");
            this.gptFiles = SqlTask.GetFilePaths(FileType.Gpt, this.gptFileNames);
        }

        [Test]
        public void ChatGPTFileExists([Range(1, FilesCount)] int index)
        {
            this.AssertFileExist(index - 1);
        }

        [Test]
        public void ChatGPTFileNotEmpty([Range(1, FilesCount)] int index)
        {
            this.AssertFileNotEmpty(index - 1);
        }

        private void AssertFileExist(int index)
        {
            Console.WriteLine(this.gptFiles[index]);
            var message = $"The file '{this.gptFileNames[index]}' was not found.";
            var actual = File.Exists(this.gptFiles[index]);
            Assert.IsTrue(actual, message);
        }

        private void AssertFileNotEmpty(int index)
        {
            Console.WriteLine(this.gptFiles[index]);
            var message = $"The file '{this.gptFileNames[index]}' is empty.";
            var actual = File.ReadAllText(this.gptFiles[index]).Length;
            Assert.IsTrue(actual > 0, message);
        }
    }
}
