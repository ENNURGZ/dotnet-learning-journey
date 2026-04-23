using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace BankSystem.Tests.AutocodeDB.Helpers;

public static class SqliteHelper
{
    static SqliteHelper()
    {
        var connectionString = InMemoryConnectionString();
        Connection = new SqliteConnection(connectionString);
    }

    public static SqliteConnection? Connection { get; set; }

    public static void OpenConnection()
    {
        var connectionString = new SqliteConnectionStringBuilder
        {
            Mode = SqliteOpenMode.Memory,
            ForeignKeys = true,
        }.ConnectionString;

        Connection = new SqliteConnection(connectionString);
        Connection.Open();
    }

    public static void OpenConnection(string file)
    {
        Connection!.Open();
        var connectionString = FileConnectionString(file, readOnly: true);
        using var source = new SqliteConnection(connectionString);
        source.Open();
        source.BackupDatabase(Connection);
        source.Close();
    }

    public static void OpenConnection(string importFile, string exportFile)
    {
        var exportString = FileConnectionString(exportFile);
        Connection = new SqliteConnection(exportString);
        Connection.Open();

        var importString = FileConnectionString(importFile, readOnly: true);
        using var importConnection = new SqliteConnection(importString);
        importConnection.Open();
        importConnection.BackupDatabase(Connection);
        importConnection.Close();
    }

    public static void CloseConnection()
    {
        if (Connection!.State is ConnectionState.Open)
        {
            Connection!.Close();
        }
    }

    public static int CountRows(string tableName)
    {
        var query = $"SELECT COUNT(*) FROM {tableName}";
        var countCmd = new SqliteCommand(query, Connection);
        var res = countCmd.ExecuteScalar();
        countCmd.Dispose();
        return Convert.ToInt32(res, CultureInfo.InvariantCulture);
    }

    public static int[] CountRows(string tableName, string columnName)
    {
        var query = $"SELECT {columnName}, COUNT({columnName}) FROM {tableName} GROUP BY {columnName}";
        var countCmd = new SqliteCommand(query, Connection);
        var reader = countCmd.ExecuteReader();
        var listRes = new List<int>();
        while (reader.Read())
        {
            listRes.Add(reader.GetInt32(1));
        }

        countCmd.Dispose();
        return listRes.ToArray();
    }

    public static void InsertData(string insertFile)
    {
        var queries = QueryHelper.GetQueries(insertFile);
        foreach (var query in queries)
        {
            var command = new SqliteCommand(query, SqliteHelper.Connection);
            _ = command.ExecuteNonQuery();
            command.Dispose();
        }
    }

    private static string InMemoryConnectionString() => new SqliteConnectionStringBuilder
    {
        Mode = SqliteOpenMode.Memory,
    }.ConnectionString;

    private static string FileConnectionString(string fileName, bool readOnly = false) => new SqliteConnectionStringBuilder
    {
        DataSource = fileName,
        Mode = readOnly ? SqliteOpenMode.ReadOnly : SqliteOpenMode.ReadWriteCreate,
    }.ConnectionString;
}
