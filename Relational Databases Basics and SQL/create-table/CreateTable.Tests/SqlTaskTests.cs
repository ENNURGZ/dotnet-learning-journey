using System;
using System.Collections.Generic;
using System.IO;
using AutocodeDB.Helpers;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace SqlCreateTable.Tests
{
    [TestFixture]
    public abstract class SqlTaskTests
    {
        protected const int CreateQueriesCount = 7;
        protected const string CreateQueriesFileName = "create.sql";
        protected static readonly string CreateQueriesFile = SqlTask.GetQueriesFullPath(CreateQueriesFileName);
        protected static readonly string[] CreateQueries = QueryHelper.GetQueries(CreateQueriesFile);
        protected static readonly IEnumerable<string> CreateQueriesWithForeignKeys =
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
    }
}
