using System;
using System.IO;
using AutocodeDB.Helpers;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace SqlDataInsert.Tests
{
    [TestFixture]
    public class SqlTaskTests
    {
        protected const string QueriesFileName = "insert.sql";
        protected static readonly string QueriesFile = SqlTask.GetQueriesFullPath(QueriesFileName);
        protected static readonly string[] Queries = QueryHelper.GetQueries(QueriesFile);

        [OneTimeSetUp]
        public void Setup()
        {
            SqliteHelper.OpenConnection("./DB/supermarket.db");
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            SqliteHelper.CloseConnection();
        }
    }
}